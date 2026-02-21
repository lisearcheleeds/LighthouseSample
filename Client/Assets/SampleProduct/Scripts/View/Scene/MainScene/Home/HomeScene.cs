using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
using SampleProduct.View.Scene.Common;
using SampleProduct.View.Scene.ModuleScene.GlobalHeader;
using UnityEngine;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.Home
{
    public class HomeScene : ProductCanvasMainSceneBase<HomeScene.HomeTransitionData>
    {
        [SerializeField] HomeView homeView;

        IGlobalHeaderModule globalHeaderModule;

        public override MainSceneId MainSceneId => SampleProductMainSceneId.Home;

        public class HomeTransitionData : ProductTransitionDataBase
        {
            public override MainSceneId MainSceneId => SampleProductMainSceneId.Home;
        }

        [Inject]
        public void Constructor(IGlobalHeaderModule globalHeaderModule)
        {
            this.globalHeaderModule = globalHeaderModule;
        }

        protected override UniTask OnSetup()
        {
            homeView.SubscribeEditButtonClick(OnClickEditButton);
            homeView.SubscribeOptionButtonClick(OnClickOptionButton);
            homeView.SubscribeDialogTestButtonClick(OnClickDialogTestButton);
            return base.OnSetup();
        }

        protected override async UniTask OnEnter(TransitionDataBase transitionData, TransitionType transitionType, bool isActivateScene, CancellationToken cancelToken)
        {
            await base.OnEnter(transitionData, transitionType, isActivateScene, cancelToken);
            globalHeaderModule.SetHeaderText("Home");
        }

        void OnClickEditButton()
        {
        }

        void OnClickOptionButton()
        {
        }

        void OnClickDialogTestButton()
        {
        }
    }
}