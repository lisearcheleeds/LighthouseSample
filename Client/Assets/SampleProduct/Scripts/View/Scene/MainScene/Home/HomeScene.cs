using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Core.Scene;
using SampleProduct.View.Scene.Common;
using UnityEngine;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.Home
{
    public class HomeScene : ProductCanvasMainSceneBase<HomeScene.HomeTransitionData>
    {
        [SerializeField] HomeView homeView;

        IModuleSceneManager moduleSceneManager;

        public override MainSceneId MainSceneId => SampleProductMainSceneId.Home;

        public class HomeTransitionData : ProductTransitionDataBase
        {
            public override MainSceneId MainSceneId => SampleProductMainSceneId.Home;
        }

        [Inject]
        public void Constructor(IModuleSceneManager moduleSceneManager)
        {
            this.moduleSceneManager = moduleSceneManager;
        }

        protected override async UniTask OnEnter(TransitionDataBase transitionData, TransitionType transitionType, CancellationToken cancelToken)
        {
            await base.OnEnter(transitionData, transitionType, cancelToken);

            // var globalHeaderScene = moduleSceneManager.GetCommonScene<GlobalHeaderModuleScene>();
            // globalHeaderScene.SetText("Home");
        }
    }
}