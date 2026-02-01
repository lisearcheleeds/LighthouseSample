using Lighthouse.Core.Scene;
using Product.View.Scene.SceneBase;

namespace Product.View.Scene.CommonScene.Overlay
{
    public class OverlayScene : ProductCommonCanvasSceneBase
    {
        public override CommonSceneKey CommonSceneId => ProductNameSpace.CommonSceneId.Overlay;

        public override bool IsAlwaysInAnimation => true;
        public override bool IsAlwaysOutAnimation => true;
    }
}