using Cysharp.Threading.Tasks;
using LighthouseExtends.Animation.Runtime;
using LighthouseExtends.ScreenStack;
using UnityEngine;

namespace SampleProduct.TransitionAnimationElement
{
    public sealed class TransitionAnimationElementOverlay : ScreenStackBase, IScreenStackSetup<TransitionAnimationElementPresenter, TransitionAnimationElementData>
    {
        [SerializeField] TransitionAnimationElementView transitionAnimationElementView;
        [SerializeField] LHTransitionAnimator transitionAnimator;

        public void Setup(TransitionAnimationElementPresenter presenter, TransitionAnimationElementData screenStackData)
        {
            presenter.Bind(transitionAnimationElementView, screenStackData);
        }

        public override void ResetInAnimation()
        {
            transitionAnimator.ResetInAnimation();
        }

        public override async UniTask PlayInAnimation()
        {
            await transitionAnimator.InAnimation();
        }

        public override void EndInAnimation()
        {
            transitionAnimator.EndInAnimation();
        }

        public override void ResetOutAnimation()
        {
            transitionAnimator.ResetOutAnimation();
        }

        public override async UniTask PlayOutAnimation()
        {
            await transitionAnimator.OutAnimation();
        }

        public override void EndOutAnimation()
        {
            transitionAnimator.EndOutAnimation();
        }
    }
}
