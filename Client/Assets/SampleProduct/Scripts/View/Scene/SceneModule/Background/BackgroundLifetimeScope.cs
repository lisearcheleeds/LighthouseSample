using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace SampleProduct.View.Scene.SceneModule.Background
{
    public class BackgroundLifetimeScope : LifetimeScope
    {
        [SerializeField] BackgroundSceneModule backgroundSceneModule;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<BackgroundEntryPoint>();
            builder.RegisterComponent(backgroundSceneModule);
        }
    }
}