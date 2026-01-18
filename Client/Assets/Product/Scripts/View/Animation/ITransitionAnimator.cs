using Cysharp.Threading.Tasks;

namespace Product.View.Animation
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