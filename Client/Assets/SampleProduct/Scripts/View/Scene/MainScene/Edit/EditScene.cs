using Lighthouse.Core.Scene;
using SampleProduct.View.Scene.Common;

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