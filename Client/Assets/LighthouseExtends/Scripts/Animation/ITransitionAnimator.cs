using Cysharp.Threading.Tasks;

namespace LighthouseExtends.Animation
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