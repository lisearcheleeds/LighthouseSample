using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
using LighthouseExtends.InputLayer;
using SampleProduct.Input;
using SampleProduct.Input.Layer;
using SampleProduct.LighthouseGenerated;
using SampleProduct.View.Base;
using VContainer;

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

        protected override IInputLayer CreateInputLayer(InputActions inputActions)
        {
            return new DefaultSceneInputLayer(inputActions, () => sceneSample1Presenter.TryClickBackButton());
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