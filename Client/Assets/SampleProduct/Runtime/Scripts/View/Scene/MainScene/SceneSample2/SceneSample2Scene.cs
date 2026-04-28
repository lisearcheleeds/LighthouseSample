using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
using LighthouseExtends.InputLayer;
using SampleProduct.Input;
using SampleProduct.Input.Layer;
using SampleProduct.LighthouseGenerated;
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
        public void Construct(ISceneSample2Presenter sceneSample2Presenter)
        {
            this.sceneSample2Presenter = sceneSample2Presenter;
        }

        protected override IInputLayer CreateInputLayer(InputActions inputActions)
        {
            return new DefaultSceneInputLayer(inputActions, () => sceneSample2Presenter.TryClickBackButton());
        }

        protected override UniTask OnSetup()
        {
            sceneSample2Presenter.Setup();
            return UniTask.CompletedTask;
        }

        protected override UniTask OnEnter(SceneSample2TransitionData transitionData, ISceneTransitionContext context, CancellationToken cancelToken)
        {
            sceneSample2Presenter.OnEnter();
            return UniTask.CompletedTask;
        }
    }
}
