using System;
using VContainer;
using VContainer.Unity;

namespace LighthouseExtends.Popup
{
    public class PopupEntryPoint : IStartable, IDisposable
    {
        readonly IPopupModuleProxy popupModuleProxy;
        readonly IPopupModuleImpl popupModuleImpl;

        [Inject]
        public PopupEntryPoint(IPopupModuleProxy popupModuleProxy, IPopupModuleImpl popupModuleImpl)
        {
            this.popupModuleProxy = popupModuleProxy;
            this.popupModuleImpl = popupModuleImpl;
        }

        void IStartable.Start()
        {
            popupModuleProxy.RegisterModule(popupModuleImpl);
        }

        void IDisposable.Dispose()
        {
            popupModuleProxy.UnregisterModule(popupModuleImpl);
        }
    }
}