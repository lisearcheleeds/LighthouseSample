using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Core.Scene;
using ProductNameSpace;
using SampleProduct.View.Scene.MainScene.Title;
using SampleProduct.View.Scene.SceneBase;
using UnityEngine;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.Splash
{
    public class SplashScene : ProductCanvasMainSceneBase<SplashScene.SplashTransitionData>
    {
        SceneManager sceneManager;
        ISceneModuleManager sceneModuleManager;

        [SerializeField] SplashView splashView;

        public override MainSceneId MainSceneId => SampleProductMainSceneId.Splash;

        public class SplashTransitionData : ProductTransitionDataBase
        {
            public override MainSceneId MainSceneId => SampleProductMainSceneId.Splash;
        }

        [Inject]
        public void Constructor(SceneManager sceneManager, ISceneModuleManager sceneModuleManager)
        {
            this.sceneManager = sceneManager;
            this.sceneModuleManager = sceneModuleManager;
        }

        protected override async UniTask OnEnter(TransitionDataBase transitionData, TransitionType transitionType, CancellationToken cancelToken)
        {
            await base.OnEnter(transitionData, transitionType, cancelToken);

            // splashView.SetupFirstSplashImage(sceneModuleManager.GetCommonScene<OverlaySceneModule>());
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