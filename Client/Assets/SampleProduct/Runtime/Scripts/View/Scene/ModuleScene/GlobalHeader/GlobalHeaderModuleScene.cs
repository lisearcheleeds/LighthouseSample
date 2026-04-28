using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
using LighthouseExtends.TextTable;
using SampleProduct.LighthouseGenerated;
using SampleProduct.View.Base;
using UnityEngine;

namespace SampleProduct.View.Scene.ModuleScene.GlobalHeader
{
    public class GlobalHeaderModuleScene : ProductCanvasModuleSceneBase
    {
        [SerializeField] GlobalHeaderView globalHeaderView;

        public override ModuleSceneId ModuleSceneId => SampleProductModuleSceneId.GlobalHeader;

        public void SetHeaderText(ITextData textValue)
        {
            globalHeaderView.SetText(textValue);
        }

        public override void ResetInAnimation(ISceneTransitionContext context)
        {
            if (context != null && context.TransitionType == TransitionType.Cross)
            {
                return;
            }

            base.ResetInAnimation(context);
        }

        protected override async UniTask InAnimation(ISceneTransitionContext context)
        {
            if (context != null && context.TransitionType == TransitionType.Cross)
            {
                return;
            }

            await base.InAnimation(context);
        }

        protected override async UniTask OutAnimation(ISceneTransitionContext context)
        {
            if (context != null && context.TransitionType == TransitionType.Cross)
            {
                return;
            }

            await base.OutAnimation(context);
        }
    }
}
