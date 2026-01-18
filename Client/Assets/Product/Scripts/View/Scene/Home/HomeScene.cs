using Lighthouse.Core.Scene;
using Product.LighthouseOverride;

namespace Product.View.Scene.Home
{
    public class HomeScene : ProductMainCanvasSceneBase<HomeScene.HomeTransitionData>
    {
        public override MainSceneKey MainSceneId => ProductNameSpace.MainSceneId.Home;

        public class HomeTransitionData : ProductTransitionDataBase
        {
            public override MainSceneKey MainSceneKey => ProductNameSpace.MainSceneId.Home;
        }
    }
}