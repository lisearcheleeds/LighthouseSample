using Lighthouse.Core.Scene;
using Product.View.Scene.SceneBase;

namespace Product.View.Scene.MainScene.Edit
{
    public class EditScene : ProductMainCanvasSceneBase<EditScene.EditTransitionData>
    {
        public override MainSceneKey MainSceneId => ProductNameSpace.MainSceneId.Edit;

        public class EditTransitionData : ProductTransitionDataBase
        {
            public override MainSceneKey MainSceneKey => ProductNameSpace.MainSceneId.Edit;
        }
    }
}