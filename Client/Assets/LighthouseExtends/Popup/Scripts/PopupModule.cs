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

        UniTask IPopupModule.EnqueuePopup(IPopupData popupData)
        {
            return popupManager.EnqueuePopup(popupData);
        }

        UniTask IPopupModule.OpenPopup()
        {
            return popupManager.OpenPopup();
        }

        UniTask IPopupModule.OpenPopup(IPopupData popupData)
        {
            return popupManager.OpenPopup(popupData);
        }

        UniTask IPopupModule.ClosePopup()
        {
            return popupManager.ClosePopup();
        }

        UniTask IPopupModule.ClosePopup(IPopupData popupData)
        {
            return popupManager.ClosePopup(popupData);
        }

        UniTask IPopupModule.ClearAllPopup()
        {
            return popupManager.ClearAllPopup();
        }

        UniTask IPopupModule.ClearCurrentAllPopup()
        {
            return popupManager.ClearCurrentAllPopup();
        }
    }
}
