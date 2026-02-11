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
        IModuleSceneManager moduleSceneManager;

        [SerializeField] SplashView splashView;

        public override MainSceneId MainSceneId => SampleProductMainSceneId.Splash;

        public class SplashTransitionData : ProductTransitionDataBase
        {
            public override MainSceneId MainSceneId => SampleProductMainSceneId.Splash;
        }

        [Inject]
        public void Constructor(SceneManager sceneManager, IModuleSceneManager moduleSceneManager)
        {
            this.sceneManager = sceneManager;
            this.moduleSceneManager = moduleSceneManager;
        }

        protected override async UniTask OnEnter(TransitionDataBase transitionData, TransitionType transitionType, CancellationToken cancelToken)
        {
            await base.OnEnter(transitionData, transitionType, cancelToken);

            // splashView.SetupFirstSplashImage(moduleSceneManager.GetCommonScene<OverlayModuleScene>());
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