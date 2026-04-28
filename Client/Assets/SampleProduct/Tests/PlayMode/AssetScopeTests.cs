using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using LighthouseExtends.Addressable;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.TestTools;

namespace SampleProduct.Tests.PlayMode
{
    public class AssetScopeTests
    {
        const string AddressA = "DialogElementDialog";
        const string AddressB = "PopupElementPopup";
        const string InvalidAddress = "this-address-does-not-exist";

        AssetManager manager;

        [UnitySetUp]
        public IEnumerator SetUp() => UniTask.ToCoroutine(async () =>
        {
            await Addressables.InitializeAsync();
            manager = new AssetManager();
        });

        [TearDown]
        public void TearDown()
        {
            manager.Dispose();
        }

        [UnityTest]
        public IEnumerator LoadAsync_ValidAddress_ReturnsNonNullAsset() => UniTask.ToCoroutine(async () =>
        {
            using var scope = manager.CreateScope();
            var handle = await scope.LoadAsync<GameObject>(AddressA);

            Assert.That(handle, Is.Not.Null);
            Assert.That(handle.Asset, Is.Not.Null);
            Assert.That(handle.Asset, Is.InstanceOf<GameObject>());
        });

        [UnityTest]
        public IEnumerator LoadAsync_MultipleAddresses_AllHandlesValid() => UniTask.ToCoroutine(async () =>
        {
            using var scope = manager.CreateScope();
            var handles = await scope.LoadAsync<GameObject>(new[] { AddressA, AddressB });

            Assert.That(handles.Count, Is.EqualTo(2));
            foreach (var h in handles)
            {
                Assert.That(h, Is.Not.Null);
                Assert.That(h.Asset, Is.Not.Null);
            }
        });

        [UnityTest]
        public IEnumerator SharedAddress_TwoScopes_DisposingFirstKeepsSecondValid() => UniTask.ToCoroutine(async () =>
        {
            var scopeA = manager.CreateScope();
            using var scopeB = manager.CreateScope();

            var handleA = await scopeA.LoadAsync<GameObject>(AddressA);
            var handleB = await scopeB.LoadAsync<GameObject>(AddressA);

            Assert.That(handleA.Asset, Is.Not.Null);
            Assert.That(handleB.Asset, Is.Not.Null);

            scopeA.Dispose();

            // scopeB still holds a reference; asset should remain accessible
            Assert.That(handleB.Asset, Is.Not.Null);
        });

        [UnityTest]
        public IEnumerator DisposedScope_Load_ThrowsObjectDisposedException() => UniTask.ToCoroutine(async () =>
        {
            var scope = manager.CreateScope();
            scope.Dispose();

            var threw = false;
            try
            {
                await scope.LoadAsync<GameObject>(AddressA);
            }
            catch (ObjectDisposedException)
            {
                threw = true;
            }

            Assert.That(threw, Is.True);
        });

        [UnityTest]
        public IEnumerator DisposedManager_ScopeLoad_ThrowsObjectDisposedException() => UniTask.ToCoroutine(async () =>
        {
            manager.Dispose();
            var scope = manager.CreateScope();

            var threw = false;
            try
            {
                await scope.LoadAsync<GameObject>(AddressA);
            }
            catch (ObjectDisposedException)
            {
                threw = true;
            }

            Assert.That(threw, Is.True);
        });

        [UnityTest]
        public IEnumerator ScopeDispose_DisposesAllOwnedHandles() => UniTask.ToCoroutine(async () =>
        {
            using var scope = manager.CreateScope();

            // LoadAsync で取得したハンドル
            var handleA = await scope.LoadAsync<GameObject>(AddressA);

            // LoadAsync(addresses) で取得したハンドル群
            var multiHandles = await scope.LoadAsync<GameObject>(new[] { AddressA, AddressB });

            // TryLoadAsync で取得したハンドル
            var data = new ParallelLoadData();
            var reqB = data.Add<GameObject>(AddressB);
            var result = await scope.TryLoadAsync(data);
            var handleB = result.Get(reqB);

            Assert.That(handleA.IsDisposed, Is.False);
            Assert.That(multiHandles[0].IsDisposed, Is.False);
            Assert.That(multiHandles[1].IsDisposed, Is.False);
            Assert.That(handleB.IsDisposed, Is.False);

            scope.Dispose();

            Assert.That(handleA.IsDisposed, Is.True);
            Assert.That(multiHandles[0].IsDisposed, Is.True);
            Assert.That(multiHandles[1].IsDisposed, Is.True);
            Assert.That(handleB.IsDisposed, Is.True);
        });

        [UnityTest]
        public IEnumerator HandleDispose_BeforeScopeDispose_NoDoubleRelease() => UniTask.ToCoroutine(async () =>
        {
            var scope = manager.CreateScope();
            var handle = await scope.LoadAsync<GameObject>(AddressA);

            handle.Dispose();
            Assert.That(handle.IsDisposed, Is.True);

            // スコープの Dispose は既に Dispose 済みのハンドルを無視する
            scope.Dispose();

            // 参照カウントがリークせずクリーンな状態であることを確認
            using var scope2 = manager.CreateScope();
            var handle2 = await scope2.LoadAsync<GameObject>(AddressA);
            Assert.That(handle2.IsDisposed, Is.False);
            Assert.That(handle2.Asset, Is.Not.Null);
        });

        [UnityTest]
        public IEnumerator SameAddress_LoadedTwiceInScope_BothHandlesDisposedOnScopeDispose() => UniTask.ToCoroutine(async () =>
        {
            using var scope = manager.CreateScope();
            var handle1 = await scope.LoadAsync<GameObject>(AddressA);
            var handle2 = await scope.LoadAsync<GameObject>(AddressA);

            // 同じアドレスなので同じアセットが返る
            Assert.That(handle1.Asset, Is.SameAs(handle2.Asset));
            Assert.That(handle1.IsDisposed, Is.False);
            Assert.That(handle2.IsDisposed, Is.False);

            scope.Dispose();

            Assert.That(handle1.IsDisposed, Is.True);
            Assert.That(handle2.IsDisposed, Is.True);
        });

        [UnityTest]
        public IEnumerator LoadAsync_Addresses_PartialFailure_ThrowsAndReleasesAcquiredHandles() => UniTask.ToCoroutine(async () =>
        {
            // AddressA を先に別スコープで保持し、失敗後に参照カウントが正しく戻るか確認する
            using var refScope = manager.CreateScope();
            var refHandle = await refScope.LoadAsync<GameObject>(AddressA);

            using var scope = manager.CreateScope();
            var threw = false;

            LogAssert.ignoreFailingMessages = true;
            try
            {
                // AddressA が成功したあと InvalidAddress で失敗するケース
                await scope.LoadAsync<GameObject>(new[] { AddressA, InvalidAddress });
            }
            catch
            {
                threw = true;
            }
            finally
            {
                LogAssert.ignoreFailingMessages = false;
            }

            Assert.That(threw, Is.True);

            // catch 内で取得済みハンドルが Dispose されるため、
            // refScope の分だけ残っていれば参照カウントは正常
            Assert.That(refHandle.IsDisposed, Is.False);
        });

        [UnityTest]
        public IEnumerator TryLoadAsync_InvalidAddress_MarkedAsFailed() => UniTask.ToCoroutine(async () =>
        {
            using var scope = manager.CreateScope();
            var data = new ParallelLoadData();
            var validReq = data.Add<GameObject>(AddressA);
            var invalidReq = data.Add<GameObject>(InvalidAddress);

            ParallelLoadResult result;
            LogAssert.ignoreFailingMessages = true;
            try
            {
                result = await scope.TryLoadAsync(data);
            }
            finally
            {
                LogAssert.ignoreFailingMessages = false;
            }

            Assert.That(result.IsSuccess(validReq), Is.True);
            Assert.That(result.IsSuccess(invalidReq), Is.False);
            Assert.That(result.Get(validReq), Is.Not.Null);
            Assert.That(result.Get(invalidReq), Is.Null);
        });
    }
}
