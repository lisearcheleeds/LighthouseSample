using Lighthouse.Core.Scene;
using ProductNameSpace;
using SampleProduct.View.Scene.SceneBase;

namespace SampleProduct.View.Scene.MainScene.Edit
{
    public class EditScene : ProductMainCanvasSceneBase<EditScene.EditTransitionData>
    {
        public override MainSceneId MainSceneId => SampleProductMainSceneId.Edit;

        public class EditTransitionData : ProductTransitionDataBase
        {
            public override MainSceneId MainSceneId => SampleProductMainSceneId.Edit;
        }
    }
}