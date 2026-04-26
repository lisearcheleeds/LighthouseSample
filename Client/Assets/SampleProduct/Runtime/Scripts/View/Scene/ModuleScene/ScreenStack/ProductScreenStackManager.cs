using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using LighthouseExtends.Addressable;
using LighthouseExtends.ScreenStack;
using VContainer;

namespace SampleProduct.View.Scene.ModuleScene.ScreenStack
{
    public class ProductScreenStackManager : ScreenStackManager
    {
        ILHAssetManager assetManager;
        readonly Dictionary<IScreenStackData, ILHAssetScope> scopes = new();

        [Inject]
        public void Construct(ILHAssetManager assetManager)
        {
            this.assetManager = assetManager;
        }

        protected override async UniTask OpenScreenStackCore(bool isPlayInAnimation)
        {
            var data = ScreenStackDataList?.LastOrDefault();
            if (data != null)
            {
                if (scopes.ContainsKey(data))
                {
                    throw new InvalidOperationException($"[ScreenStackManager] ScreenStack is already open: {data.GetType().Name}");
                }
                scopes[data] = assetManager.CreateScope();
            }

            try
            {
                await base.OpenScreenStackCore(isPlayInAnimation);
            }
            catch
            {
                if (data != null)
                {
                    DisposeScope(data);
                }
                throw;
            }
        }

        protected override async UniTask ResumeOpenScreenStacksCore(bool isPlayInAnimation)
        {
            if (ScreenStackDataList != null)
            {
                foreach (var data in ScreenStackDataList)
                {
                    scopes[data] = assetManager.CreateScope();
                }
            }

            // On failure, base calls ClearCurrentAllScreenStackCore which disposes scopes via our override.
            await base.ResumeOpenScreenStacksCore(isPlayInAnimation);
        }

        protected override async UniTask CloseScreenStackCore(IScreenStackData screenStackData)
        {
            await base.CloseScreenStackCore(screenStackData);
            DisposeScope(screenStackData);
        }

        protected override async UniTask CloseScreenStackCore()
        {
            var data = ScreenStackDataList?.LastOrDefault();
            await base.CloseScreenStackCore();
            if (data != null)
            {
                DisposeScope(data);
            }
        }

        protected override async UniTask ClearCurrentAllScreenStackCore()
        {
            await base.ClearCurrentAllScreenStackCore();
            DisposeAllScopes();
        }

        protected override void ForceDisposeAll()
        {
            base.ForceDisposeAll();
            DisposeAllScopes();
        }

        void DisposeScope(IScreenStackData data)
        {
            if (scopes.TryGetValue(data, out var scope))
            {
                scope.Dispose();
                scopes.Remove(data);
            }
        }

        void DisposeAllScopes()
        {
            foreach (var scope in scopes.Values)
            {
                scope.Dispose();
            }
            scopes.Clear();
        }
    }
}
