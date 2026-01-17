using Lighthouse.Core.Scene;
using Lighthouse.Core.Scene.SceneBase;
using Product.View.Scene.Common;

namespace Product.View.Scene.Edit
{
    public class EditScene : MainSceneBase<EditScene.EditTransitionData>
    {
        public override MainSceneKey MainSceneId => ProductNameSpace.MainSceneId.Edit;

        public class EditTransitionData : ProductTransitionDataBase
        {
            public override MainSceneKey MainSceneKey => ProductNameSpace.MainSceneId.Edit;
        }
    }
}