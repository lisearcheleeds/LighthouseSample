using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Core.Scene;
using Lighthouse.Core.Scene.SceneBase;
using Product.View.Scene.Common;
using Product.View.Scene.Overlay;
using Product.View.Scene.Title;
using UnityEngine;
using VContainer;

namespace Product.View.Scene.Splash
{
    public class SplashScene : MainCanvasSceneBase<SplashScene.SplashTransitionData>
    {
        SceneManager sceneManager;
        ICommonSceneManager commonSceneManager;

        [SerializeField] SplashView splashView;

        public override MainSceneKey MainSceneId => ProductNameSpace.MainSceneId.Splash;

        public class SplashTransitionData : ProductTransitionDataBase
        {
            public override MainSceneKey MainSceneKey => ProductNameSpace.MainSceneId.Splash;
        }

        [Inject]
        public void Constructor(SceneManager sceneManager, ICommonSceneManager commonSceneManager)
        {
            this.sceneManager = sceneManager;
            this.commonSceneManager = commonSceneManager;
        }

        public override async UniTask Enter(TransitionDataBase transitionData, TransitionType transitionType, CancellationToken cancelToken)
        {
            await base.Enter(transitionData, transitionType, cancelToken);

            splashView.SetupFirstSplashImage(commonSceneManager.GetCommonScene<OverlayScene>());
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