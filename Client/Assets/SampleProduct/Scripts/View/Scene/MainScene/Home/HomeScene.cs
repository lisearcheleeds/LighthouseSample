using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Core.Scene;
using ProductNameSpace;
using SampleProduct.View.Scene.SceneBase;
using UnityEngine;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.Home
{
    public class HomeScene : ProductMainCanvasSceneBase<HomeScene.HomeTransitionData>
    {
        [SerializeField] HomeView homeView;

        ISceneModuleManager sceneModuleManager;

        public override MainSceneId MainSceneId => SampleProductMainSceneId.Home;

        public class HomeTransitionData : ProductTransitionDataBase
        {
            public override MainSceneId MainSceneId => SampleProductMainSceneId.Home;
        }

        [Inject]
        public void Constructor(ISceneModuleManager sceneModuleManager)
        {
            this.sceneModuleManager = sceneModuleManager;
        }

        protected override async UniTask OnEnter(TransitionDataBase transitionData, TransitionType transitionType, CancellationToken cancelToken)
        {
            await base.OnEnter(transitionData, transitionType, cancelToken);

            // var globalHeaderScene = sceneModuleManager.GetCommonScene<GlobalHeaderSceneModule>();
            // globalHeaderScene.SetText("Home");
        }
    }
}