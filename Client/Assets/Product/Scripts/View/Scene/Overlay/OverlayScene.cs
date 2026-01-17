using Lighthouse.Core.Scene;
using Lighthouse.Core.Scene.SceneBase;

namespace Product.View.Scene.Overlay
{
    public class OverlayScene : CommonCanvasSceneBase
    {
        public override CommonSceneKey CommonSceneId => ProductNameSpace.CommonSceneId.Overlay;

        public override bool IsAlwaysInAnimation => true;
        public override bool IsAlwaysOutAnimation => true;
    }
}