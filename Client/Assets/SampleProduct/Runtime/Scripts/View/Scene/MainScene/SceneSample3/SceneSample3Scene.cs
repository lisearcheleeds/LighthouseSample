using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
using LighthouseExtends.InputLayer;
using SampleProduct.Input;
using SampleProduct.Input.Layer;
using SampleProduct.LighthouseGenerated;
using SampleProduct.View.Base;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.SceneSample3
{
    public sealed class SceneSample3Scene : ProductCanvasMainSceneBase<SceneSample3Scene.SceneSample3TransitionData>
    {
        ISceneSample3Presenter sceneSample3Presenter;

        public override MainSceneId MainSceneId => SampleProductMainSceneId.SceneSample3;

        public sealed class SceneSample3TransitionData : ProductTransitionDataBase
        {
            public override MainSceneId MainSceneId => SampleProductMainSceneId.SceneSample3;

            public int ChoiceData { get; }

            public SceneSample3TransitionData(int choiceData)
            {
                ChoiceData = choiceData;
            }
        }

        [Inject]
        public void Constructor(ISceneSample3Presenter sceneSample3Presenter)
        {
            this.sceneSample3Presenter = sceneSample3Presenter;
        }

        protected override IInputLayer CreateInputLayer(InputActions inputActions)
        {
            return new DefaultSceneInputLayer(inputActions, () => sceneSample3Presenter.TryClickBackButton());
        }

        protected override UniTask OnSetup()
        {
            sceneSample3Presenter.Setup();
            return UniTask.CompletedTask;
        }

        protected override UniTask OnEnter(SceneSample3TransitionData transitionData, ISceneTransitionContext context, CancellationToken cancelToken)
        {
            sceneSample3Presenter.OnEnter();
            sceneSample3Presenter.ApplyChoiceData(transitionData.ChoiceData);
            return UniTask.CompletedTask;
        }
    }
}
