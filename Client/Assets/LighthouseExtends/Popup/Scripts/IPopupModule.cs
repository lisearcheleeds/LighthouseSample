using Cysharp.Threading.Tasks;

namespace LighthouseExtends.Popup
{
    public interface IPopupModule
    {
        UniTask EnqueuePopup(IPopupData popupData);
        UniTask OpenPopup();
        UniTask OpenPopup(IPopupData popupData);

        UniTask ClosePopup();
        UniTask ClosePopup(IPopupData popupData);

        UniTask ClearAllPopup();
        UniTask ClearCurrentAllPopup();
    }
}
