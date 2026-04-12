using Cysharp.Threading.Tasks;
using LighthouseExtends.Animation.Runtime;
using LighthouseExtends.ScreenStack;
using UnityEngine;

namespace SampleProduct.ButtonElement
{
    public sealed class ButtonElementOverlay : ScreenStackBase, IScreenStackSetup<ButtonElementPresenter, ButtonElementData>
    {
        [SerializeField] ButtonElementView buttonElementView;
        [SerializeField] LHTransitionAnimator transitionAnimator;

        public void Setup(ButtonElementPresenter presenter, ButtonElementData screenStackData)
        {
            presenter.Bind(buttonElementView, screenStackData);
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
