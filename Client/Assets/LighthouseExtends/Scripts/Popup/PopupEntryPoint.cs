using System;
using VContainer;
using VContainer.Unity;

namespace LighthouseExtends.Popup
{
    public class PopupEntryPoint : IStartable, IDisposable
    {
        readonly IPopupModuleProxy popupModuleProxy;
        readonly IPopupModuleImpl popupModule;

        [Inject]
        public PopupEntryPoint(IPopupModuleProxy popupModuleProxy, IPopupModuleImpl popupModule)
        {
            this.popupModuleProxy = popupModuleProxy;
            this.popupModule = popupModule;
        }

        void IStartable.Start()
        {
            popupModuleProxy.RegisterModule(popupModule);
        }

        void IDisposable.Dispose()
        {
            popupModuleProxy.UnregisterModule(popupModule);
        }
    }
}