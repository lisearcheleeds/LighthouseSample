using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace SampleProduct.View.Scene.ModuleScene.Overlay
{
    public class OverlayLifetimeScope : LifetimeScope
    {
        [SerializeField] OverlayModuleScene overlayModuleScene;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<OverlayEntryPoint>();
            builder.RegisterComponent(overlayModuleScene);

            builder.Register<OverlayModule>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}