using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
using LighthouseExtends.InputLayer;
using SampleProduct.Input;
using SampleProduct.Input.Layer;
using SampleProduct.LighthouseGenerated;
using SampleProduct.View.Base;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.Home
{
    public class HomeScene : ProductCanvasMainSceneBase<HomeScene.HomeTransitionData>
    {
        IHomePresenter homePresenter;

        public override MainSceneId MainSceneId => SampleProductMainSceneId.Home;

        public class HomeTransitionData : ProductTransitionDataBase
        {
            public override MainSceneId MainSceneId => SampleProductMainSceneId.Home;
        }

        [Inject]
        public void Constructor(IHomePresenter homePresenter)
        {
            this.homePresenter = homePresenter;
        }

        protected override IInputLayer CreateInputLayer(InputActions inputActions) => new DefaultSceneInputLayer(inputActions, null);

        protected override UniTask OnSetup()
        {
            homePresenter.Setup();
            return UniTask.CompletedTask;
        }

        protected override UniTask OnEnter(HomeTransitionData transitionData, SceneTransitionContext context, CancellationToken cancelToken)
        {
            homePresenter.OnEnter();
            return UniTask.CompletedTask;
        }
    }
}
