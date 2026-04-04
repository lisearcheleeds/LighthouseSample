using Cysharp.Threading.Tasks;

namespace LighthouseExtends.Animation.Runtime
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