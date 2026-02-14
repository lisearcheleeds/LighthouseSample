using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace LighthouseExtends.Popup
{
    public class PopupLifetimeScope: LifetimeScope
    {
        [SerializeField] PopupModuleScene popupModuleScene;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<PopupEntryPoint>();
            builder.RegisterComponent(popupModuleScene);

            builder.Register<PopupModule>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}