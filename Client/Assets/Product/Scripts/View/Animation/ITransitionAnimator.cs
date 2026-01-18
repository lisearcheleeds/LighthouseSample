using Cysharp.Threading.Tasks;

namespace Product.View.Animation
{
    public interface ITransitionAnimator
    {
        UniTask ResetAnimation();
        UniTask InAnimation();
        UniTask OutAnimation();
    }
}