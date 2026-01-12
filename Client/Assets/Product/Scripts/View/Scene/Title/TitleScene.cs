using Lighthouse.Core.Scene;

namespace Product.View.Title
{
    public class TitleScene : MainSceneBase<TitleScene.TitleTransitionData>
    {
        public override MainSceneKey MainSceneId => ProductNameSpace.MainSceneId.Title;

        public class TitleTransitionData : TransitionDataBase
        {
            public override MainSceneKey MainSceneKey => ProductNameSpace.MainSceneId.Title;
        }
    }
}