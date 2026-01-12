using Lighthouse.Core.Scene;

namespace Product.View.Edit
{
    public class EditScene : MainSceneBase<EditScene.EditTransitionData>
    {
        public override MainSceneKey MainSceneId => ProductNameSpace.MainSceneId.Edit;

        public class EditTransitionData : TransitionDataBase
        {
            public override MainSceneKey MainSceneKey => ProductNameSpace.MainSceneId.Edit;
        }
    }
}