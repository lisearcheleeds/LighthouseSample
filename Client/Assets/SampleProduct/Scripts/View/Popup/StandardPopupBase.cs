using Cysharp.Threading.Tasks;
using LighthouseExtends.Animation;
using LighthouseExtends.Popup;
using UnityEngine;

namespace SampleProduct.View.Popup
{
    public abstract class StandardPopupBase : PopupBase
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