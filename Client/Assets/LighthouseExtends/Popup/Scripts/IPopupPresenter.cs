using Cysharp.Threading.Tasks;

namespace LighthouseExtends.Popup
{
    public interface IPopupPresenter
    {
        IPopupData PopupData { get; }

        UniTask OnEnter(IPopupData popupData, IPopup popup, bool isResume);
        UniTask OnLeave();
    }
}