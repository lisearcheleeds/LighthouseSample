using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
using SampleProduct.View.Base;
using SampleProduct.View.Scene.ModuleScene.GlobalHeader;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.Home
{
    public class HomeScene : ProductCanvasMainSceneBase<HomeScene.HomeTransitionData>
    {
        IGlobalHeaderModule globalHeaderModule;
        IHomePresenter homePresenter;

        public override MainSceneId MainSceneId => SampleProductMainSceneId.Home;

        public class HomeTransitionData : ProductTransitionDataBase
        {
            public override MainSceneId MainSceneId => SampleProductMainSceneId.Home;
        }

        [Inject]
        public void Constructor(
            IGlobalHeaderModule globalHeaderModule,
            IHomePresenter homePresenter)
        {
            this.globalHeaderModule = globalHeaderModule;
            this.homePresenter = homePresenter;
        }

        protected override UniTask OnSetup()
        {
            homePresenter.Setup();
            return UniTask.CompletedTask;
        }

        protected override async UniTask OnEnter(TransitionDataBase transitionData, TransitionType transitionType, bool isActivateScene, CancellationToken cancelToken)
        {
            await base.OnEnter(transitionData, transitionType, isActivateScene, cancelToken);
            globalHeaderModule.SetHeaderText("Home");
        }
    }
}