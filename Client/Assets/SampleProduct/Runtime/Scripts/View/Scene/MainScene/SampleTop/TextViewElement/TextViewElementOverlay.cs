using Cysharp.Threading.Tasks;
using LighthouseExtends.Animation.Runtime;
using LighthouseExtends.ScreenStack;
using SampleProduct.View.Base;
using UnityEngine;

namespace SampleProduct.View.Scene.MainScene.SampleTop.TextViewElement
{
    public sealed class TextViewElementOverlay : ProductScreenStackBase, IScreenStackSetup<TextViewElementPresenter, TextViewElementData>
    {
        [SerializeField] TextViewElementView textViewElementView;
        [SerializeField] LHTransitionAnimator transitionAnimator;

        public void Setup(TextViewElementPresenter elementPresenter, TextViewElementData screenStackElementData)
        {
            elementPresenter.Bind(textViewElementView, screenStackElementData);
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
