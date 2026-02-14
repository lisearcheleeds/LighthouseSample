using System;

namespace LighthouseExtends.Popup
{
    public class PopupModuleProxy : IPopupModule, IPopupModuleProxy
    {
        IPopupModuleImpl module;

        void IPopupModuleProxy.RegisterModule(IPopupModuleImpl module)
        {
            if (this.module != null)
            {
                throw new ArgumentException($"Duplicate register");
            }

            this.module = module;
        }

        void IPopupModuleProxy.UnregisterModule(IPopupModuleImpl module)
        {
            if (this.module != module)
            {
                throw new ArgumentException($"No match impl");
            }

            this.module = null;
        }
    }
}