
using Lighthouse.Scene;
using SampleProduct.View.Base;

namespace SampleProduct.View.Scene.MainScene.SceneSample1
{
    public class SceneSample1Scene : ProductCanvasMainSceneBase<SceneSample1Scene.SceneSample1TransitionData>
    {
        public override MainSceneId MainSceneId => SampleProductMainSceneId.SceneSample1;

        public class SceneSample1TransitionData : ProductTransitionDataBase
        {
            public override MainSceneId MainSceneId => SampleProductMainSceneId.SceneSample1;
        }
    }
}