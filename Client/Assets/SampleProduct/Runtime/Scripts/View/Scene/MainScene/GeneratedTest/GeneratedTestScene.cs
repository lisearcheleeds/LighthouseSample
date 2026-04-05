using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
using Lighthouse.Scene.SceneBase;
using SampleProduct.LighthouseGenerated;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.GeneratedTest
{
    public class GeneratedTestScene : CanvasMainSceneBase<GeneratedTestScene.GeneratedTestTransitionData>
    {
        IGeneratedTestPresenter generatedTestPresenter;

        public override MainSceneId MainSceneId => SampleProductMainSceneId.GeneratedTest;

        public class GeneratedTestTransitionData : TransitionDataBase
        {
            public override MainSceneId MainSceneId => SampleProductMainSceneId.GeneratedTest;
        }

        [Inject]
        public void Construct(IGeneratedTestPresenter generatedTestPresenter)
        {
            this.generatedTestPresenter = generatedTestPresenter;
        }

        protected override UniTask OnSetup()
        {
            generatedTestPresenter.Setup();
            return UniTask.CompletedTask;
        }

        protected override UniTask OnEnter(GeneratedTestTransitionData transitionData, SceneTransitionContext context, CancellationToken cancelToken)
        {
            generatedTestPresenter.OnEnter();
            return UniTask.CompletedTask;
        }
    }
}
