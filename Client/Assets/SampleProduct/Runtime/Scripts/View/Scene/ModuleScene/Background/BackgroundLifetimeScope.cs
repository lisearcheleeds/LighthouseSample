using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace SampleProduct.View.Scene.ModuleScene.Background
{
    public class BackgroundLifetimeScope : LifetimeScope
    {
        [SerializeField] BackgroundModuleScene backgroundModuleScene;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<BackgroundEntryPoint>();
            builder.RegisterComponent(backgroundModuleScene);

            builder.Register<BackgroundModule>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}