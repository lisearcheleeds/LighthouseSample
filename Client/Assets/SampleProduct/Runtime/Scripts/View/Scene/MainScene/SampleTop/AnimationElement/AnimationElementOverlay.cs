using Cysharp.Threading.Tasks;
using LighthouseExtends.Animation.Runtime;
using LighthouseExtends.ScreenStack;
using UnityEngine;

namespace SampleProduct.View.Scene.MainScene.SampleTop.AnimationElement
{
    public sealed class AnimationElementOverlay : ScreenStackBase, IScreenStackSetup<AnimationElementPresenter, AnimationElementData>
    {
        [SerializeField] AnimationElementView animationElementView;
        [SerializeField] LHTransitionAnimator transitionAnimator;

        public void Setup(AnimationElementPresenter presenter, AnimationElementData screenStackData)
        {
            presenter.Bind(animationElementView, screenStackData);
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
