using LighthouseExtends.Popup;
using VContainer;

namespace SampleProduct.View.Scene.ModuleScene.Popup
{
    public sealed class PopupLifetimeScope : PopupLifetimeScopeBase
    {
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder.Register<PopupEntityFactory>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<AssetLoader.AssetLoader>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}