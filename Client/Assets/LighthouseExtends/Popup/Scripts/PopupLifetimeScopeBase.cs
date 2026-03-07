using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace LighthouseExtends.Popup
{
    /// <summary>
    /// You need to register the following on this LifetimeScope:
    /// IPopupPresenterFactory
    /// IPopupFactory
    /// </summary>
    public abstract class PopupLifetimeScopeBase : LifetimeScope
    {
        [SerializeField] PopupModuleSceneBase popupModuleSceneBase;
        [SerializeField] PopupCanvasController popupCanvasController;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<PopupEntryPoint>();
            builder.RegisterComponent(popupModuleSceneBase);
            builder.RegisterComponent(popupCanvasController).AsImplementedInterfaces();

            builder.Register<PopupModule>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<PopupManager>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}