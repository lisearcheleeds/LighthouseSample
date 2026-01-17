using Cysharp.Threading.Tasks;

namespace Lighthouse.Core.Common
{
    public interface ITransitionAnimator
    {
        UniTask ResetAnimation();
        UniTask InAnimation();
        UniTask OutAnimation();
    }
}