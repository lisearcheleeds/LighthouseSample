using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace LighthouseExtends.Popup
{
    public sealed class PopupModuleProxy : IPopupModule, IPopupModuleProxy
    {
        IPopupModuleImpl module;

        UniTask IPopupModule.EnqueuePopup(IPopupData popupData, CancellationToken token)
        {
            return module?.EnqueuePopup(popupData, token) ?? UniTask.CompletedTask;
        }

        UniTask IPopupModule.OpenPopup(CancellationToken token)
        {
            return module?.OpenPopup(token) ?? UniTask.CompletedTask;
        }

        UniTask IPopupModule.OpenPopup(IPopupData popupData, CancellationToken token)
        {
            return module?.OpenPopup(popupData, token) ?? UniTask.CompletedTask;
        }

        UniTask IPopupModule.ClosePopup(CancellationToken token)
        {
            return module?.ClosePopup(token) ?? UniTask.CompletedTask;
        }

        UniTask IPopupModule.ClosePopup(IPopupData popupData, CancellationToken token)
        {
            return module?.ClosePopup(popupData, token) ?? UniTask.CompletedTask;
        }

        UniTask IPopupModule.ClearAllPopup(CancellationToken token)
        {
            return module?.ClearAllPopup(token) ?? UniTask.CompletedTask;
        }

        UniTask IPopupModule.ClearCurrentAllPopup(CancellationToken token)
        {
            return module?.ClearCurrentAllPopup(token) ?? UniTask.CompletedTask;
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