using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Core.Scene;
using Product.View.Title;
using UnityEngine;
using VContainer;

namespace Product.View.Splash
{
    public class SplashScene : MainCanvasSceneBase<SplashScene.SplashTransitionData>
    {
        SceneManager sceneManager;

        [SerializeField] SplashView splashView;

        public override MainSceneKey MainSceneId => ProductNameSpace.MainSceneId.Splash;

        public class SplashTransitionData : TransitionDataBase
        {
            public override MainSceneKey MainSceneKey => ProductNameSpace.MainSceneId.Splash;
        }

        [Inject]
        public void Constructor(SceneManager sceneManager)
        {
            this.sceneManager = sceneManager;
        }

        public override async UniTask Enter(TransitionDataBase transitionData, TransitionType transitionType, CancellationToken cancelToken)
        {
            await base.Enter(transitionData, transitionType, cancelToken);

            splashView.SetupFirstSplashImage();
        }

        public override void OnSceneTransitionFinished()
        {
            splashView.PlaySplashAnimation(() =>
            {
                sceneManager.TransitionScene(new TitleScene.TitleTransitionData());
            });
        }
    }
}