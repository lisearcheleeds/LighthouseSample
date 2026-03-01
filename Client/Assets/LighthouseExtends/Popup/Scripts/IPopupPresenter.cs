using Cysharp.Threading.Tasks;

namespace LighthouseExtends.Popup
{
    public interface IPopupPresenter
    {
        UniTask OnEnter(bool isResume);
        UniTask OnLeave();
    }
}