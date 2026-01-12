using Cysharp.Threading.Tasks;

namespace Product.Util
{
    public interface ISceneTransitionAnimator
    {
        UniTask ResetAnimation();
        UniTask InAnimation();
        UniTask OutAnimation();
    }
}