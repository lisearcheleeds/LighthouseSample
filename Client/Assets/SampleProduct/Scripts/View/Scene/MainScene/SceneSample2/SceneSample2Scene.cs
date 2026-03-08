using Lighthouse.Scene;
using SampleProduct.View.Base;

namespace SampleProduct.View.Scene.MainScene.SceneSample2
{
    public class SceneSample2Scene : ProductCanvasMainSceneBase<SceneSample2Scene.SceneSample2TransitionData>
    {
        public override MainSceneId MainSceneId => SampleProductMainSceneId.SceneSample2;

        public class SceneSample2TransitionData : ProductTransitionDataBase
        {
            public override MainSceneId MainSceneId => SampleProductMainSceneId.SceneSample2;
        }
    }
}