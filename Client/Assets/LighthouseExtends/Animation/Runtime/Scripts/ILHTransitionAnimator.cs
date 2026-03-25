using Cysharp.Threading.Tasks;

namespace LighthouseExtends.Animation
{
    public interface ILHTransitionAnimator
    {
        void ResetInAnimation();
        UniTask InAnimation();
        void EndInAnimation();

        void ResetOutAnimation();
        UniTask OutAnimation();
        void EndOutAnimation();
    }
}