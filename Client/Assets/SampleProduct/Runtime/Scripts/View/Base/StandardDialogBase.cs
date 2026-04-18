using Cysharp.Threading.Tasks;
using LighthouseExtends.Animation.Runtime;
using UnityEngine;

namespace SampleProduct.View.Base
{
    public abstract class StandardDialogBase : ProductScreenStackBase
    {
        [SerializeField] LHTransitionAnimator transitionAnimator;

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
