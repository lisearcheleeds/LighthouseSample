using Cysharp.Threading.Tasks;
using Lighthouse.Core.Scene;
using Lighthouse.Core.Scene.SceneBase;
using Product.View.Animation;
using UnityEngine;

namespace Product.View.Scene.SceneBase
{
    [RequireComponent(typeof(TransitionAnimatorManager))]
    public abstract class ProductMainCanvasSceneBase<TTransitionData> : MainCanvasSceneBase<TTransitionData> where TTransitionData : ProductTransitionDataBase, new()
    {
        [SerializeField] TransitionAnimatorManager transitionAnimatorManager;

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

            transitionAnimatorManager ??= GetComponent<TransitionAnimatorManager>();
        }
#endif
    }
}