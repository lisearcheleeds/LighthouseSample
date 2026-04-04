using LighthouseExtends.ScreenStack;
using SampleProduct.Infrastructure.AssetLoader;
using SampleProduct.LighthouseGenerated;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace SampleProduct.View.Scene.ModuleScene.ScreenStack
{
    public sealed class ScreenStackLifetimeScope : ScreenStackLifetimeScopeBase
    {
        [SerializeField] ScreenStackBackgroundInputBlocker screenStackBackgroundInputBlockerPrefab;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder.Register<ScreenStackEntityFactory>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<AssetLoader>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.RegisterComponentInNewPrefab(screenStackBackgroundInputBlockerPrefab, Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}