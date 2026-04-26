using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using LighthouseExtends.Addressable;
using LighthouseExtends.ScreenStack;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace SampleProduct.Infrastructure.AssetLoader
{
    public sealed class ProductScreenStackInstanceFactory : IProductScreenStackInstanceFactory, IDisposable
    {
        readonly IObjectResolver objectResolver;
        readonly ILHAssetManager assetManager;
        readonly Dictionary<IScreenStackData, ILHAssetScope> scopes = new();

        [Inject]
        public ProductScreenStackInstanceFactory(IObjectResolver objectResolver, ILHAssetManager assetManager)
        {
            this.objectResolver = objectResolver;
            this.assetManager = assetManager;
        }

        async UniTask<TScreenStack> IScreenStackInstanceFactory.CreateScreenStackInstance<TScreenStack>(string screenStackAddress, IScreenStackData screenStackData, CancellationToken ct)
        {
            var scope = assetManager.CreateScope();
            try
            {
                var prefab = await scope.LoadAsync<GameObject>(screenStackAddress, ct);
                var gameObject = objectResolver.Instantiate(prefab);
                var instance = gameObject.GetComponents<MonoBehaviour>().OfType<TScreenStack>().First();

                // If the same IScreenStackData instance is opened twice without closing,
                // the previous scope is overwritten and leaked. This relies on the framework
                // preventing duplicate opens for the same data instance.
                scopes[screenStackData] = scope;
                return instance;
            }
            catch
            {
                scope.Dispose();
                throw;
            }
        }

        void IProductScreenStackInstanceFactory.DisposeScope(IScreenStackData data)
        {
            if (scopes.TryGetValue(data, out var scope))
            {
                scope.Dispose();
                scopes.Remove(data);
            }
        }

        public void DisposeAllScopes()
        {
            foreach (var scope in scopes.Values)
            {
                scope.Dispose();
            }
            scopes.Clear();
        }

        public void Dispose()
        {
            DisposeAllScopes();
        }
    }
}
