using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Product.Core
{
    public class RootEntryPoint : IStartable
    {
        readonly RootLifetimeScope rootLifetimeScope;
        readonly RootLifetimeScopeSettings rootLifetimeScopeSettings;

        [Inject]
        public RootEntryPoint(RootLifetimeScope rootLifetimeScope, RootLifetimeScopeSettings rootLifetimeScopeSettings)
        {
            this.rootLifetimeScope = rootLifetimeScope;
            this.rootLifetimeScopeSettings = rootLifetimeScopeSettings;
        }

        public void Start()
        {
            using (VContainer.Unity.LifetimeScope.EnqueueParent(rootLifetimeScope))
            {
                var instance = Object.Instantiate(rootLifetimeScopeSettings.ProductLifetimeScopePrefab);
                Object.DontDestroyOnLoad(instance.gameObject);
            }
        }
    }
}