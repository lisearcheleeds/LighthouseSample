using Lighthouse.Core.Scene;
using ProductNameSpace;
using SampleProduct.View.Scene.SceneBase;

namespace SampleProduct.View.Scene.SceneModule.Overlay
{
    public class OverlaySceneModule : ProductCanvasSceneModuleBase
    {
        public override SceneModuleId SceneModuleId => SampleProductSceneModuleId.Overlay;

        public override bool IsAlwaysInAnimation => true;
        public override bool IsAlwaysOutAnimation => true;
    }
}