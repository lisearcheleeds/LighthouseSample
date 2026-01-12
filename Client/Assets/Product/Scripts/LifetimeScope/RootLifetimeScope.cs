using Product.Util;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Product
{
    public class RootLifetimeScope : LifetimeScope
    {
        [SerializeField] RootLifetimeScopeSettings rootLifetimeScopeSettings;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<RootEntryPoint>();
            builder.RegisterInstance(rootLifetimeScopeSettings);
        }
    }
}