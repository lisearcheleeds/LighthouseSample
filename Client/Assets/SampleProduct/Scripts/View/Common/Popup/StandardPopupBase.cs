using Cysharp.Threading.Tasks;
using LighthouseExtends.Animation;
using LighthouseExtends.Popup;
using UnityEngine;

namespace SampleProduct.View.Common.Popup
{
    public abstract class StandardPopupBase : PopupBase
    {
        [SerializeField] LHTransitionAnimator transitionAnimator;

        public override async UniTask InAnimation()
        {
            await transitionAnimator.InAnimation();
        }

        public override async UniTask OutAnimation()
        {
            await transitionAnimator.OutAnimation();
        }
    }
}