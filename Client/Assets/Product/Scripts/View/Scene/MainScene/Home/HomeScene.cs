using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Core.Scene;
using Product.View.Scene.SceneBase;
using ProductNameSpace;
using UnityEngine;

namespace Product.View.Scene.MainScene.Home
{
    public class HomeScene : ProductMainCanvasSceneBase<HomeScene.HomeTransitionData>
    {
        [SerializeField] HomeView homeView;

        public override MainSceneKey MainSceneId => ProductNameSpace.MainSceneId.Home;

        public class HomeTransitionData : ProductTransitionDataBase
        {
            public override MainSceneKey MainSceneKey => ProductNameSpace.MainSceneId.Home;

            protected override CommonSceneKey[] ExtendCommonSceneIds => new CommonSceneKey[] { CommonSceneId.Background, CommonSceneId.GlobalHeader, };
        }

        protected override async UniTask OnEnter(TransitionDataBase transitionData, TransitionType transitionType, CancellationToken cancelToken)
        {
            await base.OnEnter(transitionData, transitionType, cancelToken);

        }
    }
}