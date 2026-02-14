using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
using SampleProduct.View.Scene.Common;
using SampleProduct.View.Scene.MainScene.Title;
using SampleProduct.View.Scene.ModuleScene.Overlay;
using UnityEngine;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.Splash
{
    public class SplashScene : ProductCanvasMainSceneBase<SplashScene.SplashTransitionData>
    {
        ISceneManager sceneManager;
        IOverlayModule overlayModule;

        [SerializeField] SplashView splashView;

        public override MainSceneId MainSceneId => SampleProductMainSceneId.Splash;

        public class SplashTransitionData : ProductTransitionDataBase
        {
            public override MainSceneId MainSceneId => SampleProductMainSceneId.Splash;
        }

        [Inject]
        public void Constructor(ISceneManager sceneManager, IOverlayModule overlayModule)
        {
            this.sceneManager = sceneManager;
            this.overlayModule = overlayModule;
        }

        protected override async UniTask OnEnter(TransitionDataBase transitionData, TransitionType transitionType, bool isActivateScene, CancellationToken cancelToken)
        {
            await base.OnEnter(transitionData, transitionType, isActivateScene, cancelToken);

            splashView.Setup(overlayModule);
        }

        public override void OnSceneTransitionFinished(SceneTransitionDiff sceneTransitionDiff)
        {
            splashView.PlaySplashAnimation(() =>
            {
                sceneManager.TransitionScene(new TitleScene.TitleTransitionData());
            });
        }
    }
}