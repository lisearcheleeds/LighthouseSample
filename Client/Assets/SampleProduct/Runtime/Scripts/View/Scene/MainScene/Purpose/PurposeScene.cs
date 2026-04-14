using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
using SampleProduct.LighthouseGenerated;
using SampleProduct.View.Base;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.Purpose
{
    public class PurposeScene : ProductCanvasMainSceneBase<PurposeScene.PurposeTransitionData>
    {
        IPurposePresenter purposePresenter;

        public override MainSceneId MainSceneId => SampleProductMainSceneId.Purpose;

        public class PurposeTransitionData : ProductTransitionDataBase
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
