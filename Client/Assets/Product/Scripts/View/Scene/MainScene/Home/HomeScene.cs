using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Core.Scene;
using Product.View.Scene.CommonScene.GlobalHeader;
using Product.View.Scene.SceneBase;
using ProductNameSpace;
using UnityEngine;
using VContainer;

namespace Product.View.Scene.MainScene.Home
{
    public class HomeScene : ProductMainCanvasSceneBase<HomeScene.HomeTransitionData>
    {
        [SerializeField] HomeView homeView;

        ICommonSceneManager commonSceneManager;

        public override MainSceneKey MainSceneId => ProductNameSpace.MainSceneId.Home;

        public class HomeTransitionData : ProductTransitionDataBase
        {
            public override MainSceneKey MainSceneKey => ProductNameSpace.MainSceneId.Home;

            protected override CommonSceneKey[] ExtendCommonSceneIds => new CommonSceneKey[] { CommonSceneId.Background, CommonSceneId.GlobalHeader, };
        }

        [Inject]
        public void Constructor(ICommonSceneManager commonSceneManager)
        {
            this.commonSceneManager = commonSceneManager;
        }

        protected override async UniTask OnEnter(TransitionDataBase transitionData, TransitionType transitionType, CancellationToken cancelToken)
        {
            await base.OnEnter(transitionData, transitionType, cancelToken);

            var globalHeaderScene = commonSceneManager.GetCommonScene<GlobalHeaderScene>();
            globalHeaderScene.SetText("Home");
        }
    }
}