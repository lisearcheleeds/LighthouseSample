using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace SampleProduct.View.Scene.SceneModule.Overlay
{
    public class OverlayLifetimeScope : LifetimeScope
    {
        [SerializeField] OverlaySceneModule overlaySceneModule;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<OverlayEntryPoint>();
            builder.RegisterComponent(overlaySceneModule);
        }
    }
}