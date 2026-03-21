using System;
using Cysharp.Threading.Tasks;

namespace LighthouseExtends.Popup
{
    public sealed class PopupModuleProxy : IPopupModule, IPopupModuleProxy
    {
        IPopupModuleImpl module;

        UniTask IPopupModule.EnqueuePopup(IPopupData popupData)
        {
            return module?.EnqueuePopup(popupData) ?? UniTask.CompletedTask;
        }

        UniTask IPopupModule.OpenPopup()
        {
            return module?.OpenPopup() ?? UniTask.CompletedTask;
        }

        UniTask IPopupModule.OpenPopup(IPopupData popupData)
        {
            return module?.OpenPopup(popupData) ?? UniTask.CompletedTask;
        }

        UniTask IPopupModule.ClosePopup()
        {
            return module?.ClosePopup() ?? UniTask.CompletedTask;
        }

        UniTask IPopupModule.ClosePopup(IPopupData popupData)
        {
            return module?.ClosePopup(popupData) ?? UniTask.CompletedTask;
        }

        UniTask IPopupModule.ClearAllPopup()
        {
            return module?.ClearAllPopup() ?? UniTask.CompletedTask;
        }

        UniTask IPopupModule.ClearCurrentAllPopup()
        {
            return module?.ClearCurrentAllPopup() ?? UniTask.CompletedTask;
        }

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
