using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
using SampleProduct.View.Scene.Common;
using SampleProduct.View.Scene.ModuleScene.GlobalHeader;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.Home
{
    public class HomeScene : ProductCanvasMainSceneBase<HomeScene.HomeTransitionData>
    {
        IGlobalHeaderModule globalHeaderModule;
        IHomeViewController homeViewController;

        public override MainSceneId MainSceneId => SampleProductMainSceneId.Home;

        public class HomeTransitionData : ProductTransitionDataBase
        {
            public override MainSceneId MainSceneId => SampleProductMainSceneId.Home;
        }

        [Inject]
        public void Constructor(
            IGlobalHeaderModule globalHeaderModule,
            IHomeViewController homeViewController)
        {
            this.globalHeaderModule = globalHeaderModule;
            this.homeViewController = homeViewController;
        }

        protected override UniTask OnSetup()
        {
            homeViewController.Setup();
            return UniTask.CompletedTask;
        }

        protected override async UniTask OnEnter(TransitionDataBase transitionData, TransitionType transitionType, bool isActivateScene, CancellationToken cancelToken)
        {
            await base.OnEnter(transitionData, transitionType, isActivateScene, cancelToken);
            globalHeaderModule.SetHeaderText("Home");
        }
    }
}