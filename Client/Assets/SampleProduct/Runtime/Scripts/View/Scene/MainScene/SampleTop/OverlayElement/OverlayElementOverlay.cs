using Cysharp.Threading.Tasks;
using LighthouseExtends.Animation.Runtime;
using LighthouseExtends.ScreenStack;
using SampleProduct.View.Base;
using UnityEngine;

namespace SampleProduct.View.Scene.MainScene.SampleTop.OverlayElement
{
    public sealed class OverlayElementOverlay : ProductScreenStackBase, IScreenStackSetup<OverlayElementPresenter, OverlayElementData>
    {
        [SerializeField] OverlayElementView overlayElementView;
        [SerializeField] LHTransitionAnimator transitionAnimator;

        public void Setup(OverlayElementPresenter presenter, OverlayElementData screenStackData)
        {
            presenter.Bind(overlayElementView, screenStackData);
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
