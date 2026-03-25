using Cysharp.Threading.Tasks;

namespace LighthouseExtends.ScreenStack
{
    public interface IScreenStackPresenter
    {
        UniTask OnEnter(bool isResume);
        UniTask OnLeave();
    }
}