using Lighthouse.Core.Scene;
using Product.View.Scene.SceneBase;

namespace Product.View.Scene.CommonScene.Background
{
    public class BackgroundScene : ProductCommonCanvasSceneBase
    {
        public override CommonSceneKey CommonSceneId => ProductNameSpace.CommonSceneId.Background;

        public override bool IsAlwaysInAnimation => true;
        public override bool IsAlwaysOutAnimation => true;
    }
}