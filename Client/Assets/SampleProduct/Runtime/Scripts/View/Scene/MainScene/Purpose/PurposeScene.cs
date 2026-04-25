using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
using LighthouseExtends.InputLayer;
using SampleProduct.Input;
using SampleProduct.Input.Layer;
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
        public void Constructor(IPurposePresenter purposePresenter)
        {
            this.purposePresenter = purposePresenter;
        }

        protected override IInputLayer CreateInputLayer(InputActions inputActions)
        {
            return new DefaultSceneInputLayer(inputActions, () => purposePresenter.TryClickBackButton());
        }

        protected override UniTask OnSetup()
        {
            purposePresenter.Setup();
            return UniTask.CompletedTask;
        }

        protected override UniTask OnEnter(PurposeTransitionData transitionData, ISceneTransitionContext context, CancellationToken cancelToken)
        {
            purposePresenter.OnEnter();
            return UniTask.CompletedTask;
        }
    }
}
