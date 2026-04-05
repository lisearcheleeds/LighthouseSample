using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
using Lighthouse.Scene.SceneBase;
using SampleProduct.LighthouseGenerated.LighthouseGenerated;
using VContainer;

namespace SampleProduct.LighthouseGenerated.View.Scene.MainScene.GenerateSample
{
    public class GenerateSampleScene : CanvasMainSceneBase<GenerateSampleScene.GenerateSampleTransitionData>
    {
        IGenerateSamplePresenter generateSamplePresenter;

        public override MainSceneId MainSceneId => SampleProductMainSceneId.GenerateSample;

        public class GenerateSampleTransitionData : TransitionDataBase
        {
            public override MainSceneId MainSceneId => SampleProductMainSceneId.GenerateSample;
        }

        [Inject]
        public void Construct(IGenerateSamplePresenter generateSamplePresenter)
        {
            this.generateSamplePresenter = generateSamplePresenter;
        }

        protected override UniTask OnSetup()
        {
            generateSamplePresenter.Setup();
            return UniTask.CompletedTask;
        }

        protected override UniTask OnEnter(GenerateSampleTransitionData transitionData, SceneTransitionContext context, CancellationToken cancelToken)
        {
            generateSamplePresenter.OnEnter();
            return UniTask.CompletedTask;
        }
    }
}
