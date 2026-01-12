using Lighthouse.Core.Scene;

namespace Product.View.Home
{
    public class HomeScene : MainSceneBase<HomeScene.HomeTransitionData>
    {
        public override MainSceneKey MainSceneId => ProductNameSpace.MainSceneId.Home;

        public class HomeTransitionData : TransitionDataBase
        {
            public override MainSceneKey MainSceneKey => ProductNameSpace.MainSceneId.Home;
        }
    }
}