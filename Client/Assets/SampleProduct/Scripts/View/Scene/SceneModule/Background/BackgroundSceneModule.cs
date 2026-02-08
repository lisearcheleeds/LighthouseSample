using Lighthouse.Core.Scene;
using ProductNameSpace;
using SampleProduct.View.Scene.SceneBase;

namespace SampleProduct.View.Scene.SceneModule.Background
{
    public class BackgroundSceneModule : ProductCanvasSceneModuleBase
    {
        public override SceneModuleId SceneModuleId => SampleProductSceneModuleId.Background;

        public override bool IsAlwaysInAnimation => true;
        public override bool IsAlwaysOutAnimation => true;
    }
}