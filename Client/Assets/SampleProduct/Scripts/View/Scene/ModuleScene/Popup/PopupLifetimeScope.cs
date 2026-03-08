using LighthouseExtends.Popup;
using SampleProduct.Infrastructure.AssetLoader;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace SampleProduct.View.Scene.ModuleScene.Popup
{
    public sealed class PopupLifetimeScope : PopupLifetimeScopeBase
    {
        [SerializeField] PopupBackgroundInputBlocker popupBackgroundInputBlockerPrefab;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder.Register<PopupEntityFactory>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<AssetLoader>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.RegisterComponentInNewPrefab(popupBackgroundInputBlockerPrefab, Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}