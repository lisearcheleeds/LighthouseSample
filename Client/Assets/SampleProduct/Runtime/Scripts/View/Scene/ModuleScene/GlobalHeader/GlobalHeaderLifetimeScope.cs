using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace SampleProduct.View.Scene.ModuleScene.GlobalHeader
{
    public class GlobalHeaderLifetimeScope : LifetimeScope
    {
        [SerializeField] GlobalHeaderModuleScene globalHeaderModuleScene;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<GlobalHeaderEntryPoint>();
            builder.RegisterComponent(globalHeaderModuleScene);

            builder.Register<GlobalHeaderModule>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}