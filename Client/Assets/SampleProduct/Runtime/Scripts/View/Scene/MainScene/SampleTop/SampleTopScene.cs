using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
using LighthouseExtends.InputLayer;
using SampleProduct.Input;
using SampleProduct.Input.Layer;
using SampleProduct.LighthouseGenerated;
using SampleProduct.View.Base;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.SampleTop
{
    public class SampleTopScene : ProductCanvasMainSceneBase<SampleTopScene.SampleTopTransitionData>
    {
        ISampleTopPresenter sampleTopPresenter;

        public override MainSceneId MainSceneId => SampleProductMainSceneId.SampleTop;

        public class SampleTopTransitionData : ProductTransitionDataBase
        {
            public override MainSceneId MainSceneId => SampleProductMainSceneId.SampleTop;

            public TabType TargetTabType { get; private set; }

            public SampleTopTransitionData() : this(TabType.Overview)
            {
            }

            public SampleTopTransitionData(TabType targetTabType)
            {
                SetTargetTabType(targetTabType);
            }

            public void SetTargetTabType(TabType targetTabType)
            {
                TargetTabType = targetTabType;
            }
        }

        [Inject]
        public void Constructor(ISampleTopPresenter sampleTopPresenter)
        {
            this.sampleTopPresenter = sampleTopPresenter;
        }

        protected override IInputLayer CreateInputLayer(InputActions inputActions)
        {
            return new DefaultSceneInputLayer(inputActions, () => sampleTopPresenter.TryClickBackButton());
        }

        protected override UniTask OnSetup()
        {
            sampleTopPresenter.Setup();
            return UniTask.CompletedTask;
        }

        protected override UniTask OnEnter(SampleTopTransitionData transitionData, ISceneTransitionContext context, CancellationToken cancelToken)
        {
            sampleTopPresenter.OnEnter(transitionData.TargetTabType);
            return base.OnEnter(transitionData, context, cancelToken);
        }

        protected override void OnCompleteInAnimation(ISceneTransitionContext context)
        {
            sampleTopPresenter.OnCompleteInAnimation();
        }

        protected override UniTask OnLeave(ISceneTransitionContext context, CancellationToken cancelToken)
        {
            TransitionData.SetTargetTabType(sampleTopPresenter.CurrentTabType);
            return base.OnLeave(context, cancelToken);
        }
    }
}
