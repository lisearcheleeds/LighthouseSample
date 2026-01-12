using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Product.LifetimeScope
{
    public class RootLifetimeScope : VContainer.Unity.LifetimeScope
    {
        [SerializeField] RootLifetimeScopeSettings rootLifetimeScopeSettings;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<RootEntryPoint>();
            builder.RegisterInstance(rootLifetimeScopeSettings);
        }
    }
}