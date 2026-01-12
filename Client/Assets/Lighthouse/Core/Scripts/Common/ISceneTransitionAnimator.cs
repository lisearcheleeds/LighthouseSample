using Cysharp.Threading.Tasks;

namespace Lighthouse.Core.Common
{
    public interface ISceneTransitionAnimator
    {
        UniTask ResetAnimation();
        UniTask InAnimation();
        UniTask OutAnimation();
    }
}