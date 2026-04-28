using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
using SampleProduct.LighthouseGenerated;
using SampleProduct.View.Base;

namespace SampleProduct.View.Scene.ModuleScene.Overlay
{
    public class OverlayModuleScene : ProductCanvasModuleSceneBase
    {
        public override ModuleSceneId ModuleSceneId => SampleProductModuleSceneId.Overlay;

        public async UniTask PlayInAnimation()
        {
            await InAnimation(null);
        }

        public async UniTask PlayOutAnimation()
        {
            await OutAnimation(null);
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
