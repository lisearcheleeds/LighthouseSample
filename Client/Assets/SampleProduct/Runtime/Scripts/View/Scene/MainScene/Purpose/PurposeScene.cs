using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
using Lighthouse.Scene.SceneBase;
using SampleProduct.LighthouseGenerated;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.Purpose
{
    public class PurposeScene : CanvasMainSceneBase<PurposeScene.PurposeTransitionData>
    {
        IPurposePresenter purposePresenter;

        public override MainSceneId MainSceneId => SampleProductMainSceneId.Purpose;

        public class PurposeTransitionData : TransitionDataBase
        {
            public override MainSceneId MainSceneId => SampleProductMainSceneId.Purpose;
        }

        [Inject]
        public void Construct(IPurposePresenter purposePresenter)
        {
            this.purposePresenter = purposePresenter;
        }

        protected override UniTask OnSetup()
        {
            purposePresenter.Setup();
            return UniTask.CompletedTask;
        }

        protected override UniTask OnEnter(PurposeTransitionData transitionData, SceneTransitionContext context, CancellationToken cancelToken)
        {
            purposePresenter.OnEnter();
            return UniTask.CompletedTask;
        }
    }
}
