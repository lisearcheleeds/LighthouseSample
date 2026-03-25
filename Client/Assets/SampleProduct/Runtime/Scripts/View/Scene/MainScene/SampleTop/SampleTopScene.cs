using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
using SampleProduct.View.Base;
using VContainer;
using SampleProduct.LighthouseGenerated;

namespace SampleProduct.View.Scene.MainScene.SampleTop
{
    public class SampleTopScene : ProductCanvasMainSceneBase<SampleTopScene.SampleTopTransitionData>
    {
        ISampleTopPresenter sampleTopPresenter;

        public override MainSceneId MainSceneId => SampleProductMainSceneId.SampleTop;

        public class SampleTopTransitionData : ProductTransitionDataBase
        {
            public override MainSceneId MainSceneId => SampleProductMainSceneId.SampleTop;
        }

        [Inject]
        public void Constructor(ISampleTopPresenter sampleTopPresenter)
        {
            this.sampleTopPresenter = sampleTopPresenter;
        }

        protected override UniTask OnSetup()
        {
            sampleTopPresenter.Setup();
            return UniTask.CompletedTask;
        }

        protected override UniTask OnEnter(SampleTopTransitionData transitionData, SceneTransitionContext context, CancellationToken cancelToken)
        {
            sampleTopPresenter.OnEnter();
            return UniTask.CompletedTask;
        }
    }
}