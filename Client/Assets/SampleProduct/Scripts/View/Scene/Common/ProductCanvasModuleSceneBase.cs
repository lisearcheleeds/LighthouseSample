using Cysharp.Threading.Tasks;
using Lighthouse.Core.Scene;
using Lighthouse.Core.Scene.SceneBase;
using Lighthouse.Extends.Animation;
using UnityEngine;

namespace SampleProduct.View.Scene.Common
{
    [RequireComponent(typeof(LHTransitionAnimatorManager))]
    public abstract class ProductCanvasModuleSceneBase : CanvasModuleSceneBase
    {
        [SerializeField] LHTransitionAnimatorManager transitionAnimatorManager;

        public override void ResetInAnimation(TransitionType transitionType)
        {
            transitionAnimatorManager.ResetInAnimation(transitionType);
        }

        protected override async UniTask InAnimation(TransitionType transitionType, bool withStateChange)
        {
            await transitionAnimatorManager.InAnimation(transitionType);
        }

        protected override async UniTask OutAnimation(TransitionType transitionType, bool withStateChange)
        {
            await transitionAnimatorManager.OutAnimation(transitionType);
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            transitionAnimatorManager ??= GetComponent<LHTransitionAnimatorManager>();
        }
#endif
    }
}