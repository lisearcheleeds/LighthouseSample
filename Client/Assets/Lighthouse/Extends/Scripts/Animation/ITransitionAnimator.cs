using Cysharp.Threading.Tasks;

namespace Lighthouse.Extends.Animation
{
    public interface ITransitionAnimator
    {
        void ResetInAnimation();
        UniTask InAnimation();
        void EndInAnimation();

        void ResetOutAnimation();
        UniTask OutAnimation();
        void EndOutAnimation();
    }
}