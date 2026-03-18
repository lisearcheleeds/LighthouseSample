using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
using SampleProduct.View.Base;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.SceneSample2
{
    public class SceneSample2Scene : ProductCanvasMainSceneBase<SceneSample2Scene.SceneSample2TransitionData>
    {
        ISceneSample2Presenter sceneSample2Presenter;

        public override MainSceneId MainSceneId => SampleProductMainSceneId.SceneSample2;

        public class SceneSample2TransitionData : ProductTransitionDataBase
        {
            public override MainSceneId MainSceneId => SampleProductMainSceneId.SceneSample2;
        }

        [Inject]
        public void Constructor(ISceneSample2Presenter sceneSample2Presenter)
        {
            this.sceneSample2Presenter = sceneSample2Presenter;
        }

        protected override UniTask OnSetup()
        {
            sceneSample2Presenter.Setup();
            return UniTask.CompletedTask;
        }

        protected override UniTask OnEnter(
            TransitionDataBase transitionData,
            TransitionType transitionType,
            SceneTransitionDiff sceneTransitionDiff,
            CancellationToken cancelToken)
        {
            sceneSample2Presenter.OnEnter();
            return base.OnEnter(transitionData, transitionType, sceneTransitionDiff, cancelToken);
        }
    }
}