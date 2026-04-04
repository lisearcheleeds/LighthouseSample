using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
using SampleProduct.View.Base;
using VContainer;
using SampleProduct.LighthouseGenerated;

namespace SampleProduct.View.Scene.MainScene.SceneSample1
{
    public class SceneSample1Scene : ProductCanvasMainSceneBase<SceneSample1Scene.SceneSample1TransitionData>
    {
        ISceneSample1Presenter sceneSample1Presenter;

        public override MainSceneId MainSceneId => SampleProductMainSceneId.SceneSample1;

        public class SceneSample1TransitionData : ProductTransitionDataBase
        {
            public override MainSceneId MainSceneId => SampleProductMainSceneId.SceneSample1;
        }

        [Inject]
        public void Constructor(ISceneSample1Presenter sceneSample1Presenter)
        {
            this.sceneSample1Presenter = sceneSample1Presenter;
        }

        protected override UniTask OnSetup()
        {
            sceneSample1Presenter.Setup();
            return UniTask.CompletedTask;
        }

        protected override UniTask OnEnter(SceneSample1TransitionData transitionData, SceneTransitionContext context, CancellationToken cancelToken)
        {
            sceneSample1Presenter.OnEnter();
            return UniTask.CompletedTask;
        }
    }
}