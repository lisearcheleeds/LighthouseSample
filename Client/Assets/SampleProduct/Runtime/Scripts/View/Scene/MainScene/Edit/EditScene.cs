using Lighthouse.Scene;
using SampleProduct.View.Base;
using SampleProduct.LighthouseGenerated;

namespace SampleProduct.View.Scene.MainScene.Edit
{
    public class EditScene : ProductCanvasMainSceneBase<EditScene.EditTransitionData>
    {
        public override MainSceneId MainSceneId => SampleProductMainSceneId.Edit;

        public class EditTransitionData : ProductTransitionDataBase
        {
            public override MainSceneId MainSceneId => SampleProductMainSceneId.Edit;
        }
    }
}