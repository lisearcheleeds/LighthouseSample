using Lighthouse.Core.Scene;
using Product.LighthouseOverride;

namespace Product.View.Scene.Edit
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