using Lighthouse.Core.Scene;
using SampleProduct.View.Scene.Common;

namespace SampleProduct.View.Scene.ModuleScene.Overlay
{
    public class OverlayModuleScene : ProductCanvasModuleSceneBase
    {
        public override ModuleSceneId ModuleSceneId => SampleProductModuleSceneId.Overlay;

        public override bool IsAlwaysInAnimation => true;
        public override bool IsAlwaysOutAnimation => true;
    }
}