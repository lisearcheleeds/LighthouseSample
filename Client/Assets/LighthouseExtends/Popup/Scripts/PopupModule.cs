using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer;

namespace LighthouseExtends.Popup
{
    public sealed class PopupModule : IPopupModuleImpl
    {
        readonly IPopupManager popupManager;

        [Inject]
        public PopupModule(IPopupManager popupManager)
        {
            this.popupManager = popupManager;
        }

        UniTask IPopupModule.EnqueuePopup(IPopupData popupData, CancellationToken token)
        {
            return popupManager.EnqueuePopup(popupData, token);
        }

        UniTask IPopupModule.OpenPopup(CancellationToken token)
        {
            return popupManager.OpenPopup(token);
        }

        UniTask IPopupModule.OpenPopup(IPopupData popupData, CancellationToken token)
        {
            return popupManager.OpenPopup(popupData, token);
        }

        UniTask IPopupModule.ClosePopup(CancellationToken token)
        {
            return popupManager.ClosePopup(token);
        }

        UniTask IPopupModule.ClosePopup(IPopupData popupData, CancellationToken token)
        {
            return popupManager.ClosePopup(popupData, token);
        }

        UniTask IPopupModule.ClearAllPopup(CancellationToken token)
        {
            return popupManager.ClearAllPopup(token);
        }

        UniTask IPopupModule.ClearCurrentAllPopup(CancellationToken token)
        {
            return popupManager.ClearCurrentAllPopup(token);
        }
    }
}