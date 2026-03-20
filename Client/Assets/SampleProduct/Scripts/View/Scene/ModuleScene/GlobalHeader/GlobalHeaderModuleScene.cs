using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
using SampleProduct.View.Base;
using UnityEngine;

namespace SampleProduct.View.Scene.ModuleScene.GlobalHeader
{
    public class GlobalHeaderModuleScene : ProductCanvasModuleSceneBase
    {
        [SerializeField] GlobalHeaderView globalHeaderView;

        public override ModuleSceneId ModuleSceneId => SampleProductModuleSceneId.GlobalHeader;

        public void SetHeaderText(string text)
        {
            globalHeaderView.SetText(text);
        }

        public override void ResetInAnimation(SceneTransitionContext context)
        {
            if (context != null && context.TransitionType == TransitionType.Cross)
            {
                return;
            }

            base.ResetInAnimation(context);
        }

        protected override async UniTask InAnimation(SceneTransitionContext context)
        {
            if (context != null && context.TransitionType == TransitionType.Cross)
            {
                return;
            }

            await base.InAnimation(context);
        }

        protected override async UniTask OutAnimation(SceneTransitionContext context)
        {
            if (context != null && context.TransitionType == TransitionType.Cross)
            {
                return;
            }

            await base.OutAnimation(context);
        }
    }
}