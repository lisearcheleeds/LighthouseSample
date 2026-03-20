using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
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
    }
}