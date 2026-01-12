using Lighthouse.Core.Scene;
using Lighthouse.Core.Scene.SceneBase;

namespace Product.View.Scene.Title
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