using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
using Lighthouse.Scene.SceneBase;
using LighthouseExtends.Animation;
using UnityEngine;

namespace SampleProduct.View.Base
{
    [RequireComponent(typeof(LHTransitionAnimatorManager))]
    public abstract class ProductCanvasMainSceneBase<TTransitionData> : CanvasMainSceneBase<TTransitionData> where TTransitionData : ProductTransitionDataBase
    {
        [SerializeField] LHTransitionAnimatorManager transitionAnimatorManager;

        public override void ResetInAnimation(SceneTransitionContext context)
        {
            transitionAnimatorManager.ResetInAnimation();
        }

        protected override async UniTask InAnimation(SceneTransitionContext context)
        {
            await transitionAnimatorManager.InAnimation();
        }

        protected override async UniTask OutAnimation(SceneTransitionContext context)
        {
            await transitionAnimatorManager.OutAnimation();
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