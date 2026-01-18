using Lighthouse.Core.Scene;
using Product.LighthouseOverride;

namespace Product.View.Scene.Overlay
{
    public class OverlayScene : ProductCommonCanvasSceneBase
    {
        public override CommonSceneKey CommonSceneId => ProductNameSpace.CommonSceneId.Overlay;

        public override bool IsAlwaysInAnimation => true;
        public override bool IsAlwaysOutAnimation => true;
    }
}