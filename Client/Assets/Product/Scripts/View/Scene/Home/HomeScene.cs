using Lighthouse.Core.Scene;
using Lighthouse.Core.Scene.SceneBase;
using Product.View.Scene.Common;

namespace Product.View.Scene.Home
{
    public class HomeScene : MainSceneBase<HomeScene.HomeTransitionData>
    {
        public override MainSceneKey MainSceneId => ProductNameSpace.MainSceneId.Home;

        public class HomeTransitionData : ProductTransitionDataBase
        {
            public override MainSceneKey MainSceneKey => ProductNameSpace.MainSceneId.Home;
        }
    }
}