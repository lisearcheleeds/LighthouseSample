using Lighthouse.Scene;
using SampleProduct.View.Base;
using VContainer;
using SampleProduct.LighthouseGenerated;

namespace SampleProduct.View.Scene.MainScene.Splash
{
    public class SplashScene : ProductCanvasMainSceneBase<SplashScene.SplashTransitionData>
    {
        ISplashPresenter splashPresenter;

        public override MainSceneId MainSceneId => SampleProductMainSceneId.Splash;

        public class SplashTransitionData : ProductTransitionDataBase
        {
            public override MainSceneId MainSceneId => SampleProductMainSceneId.Splash;
        }

        [Inject]
        public void Constructor(ISplashPresenter splashPresenter)
        {
            this.splashPresenter = splashPresenter;
        }

        public override void OnSceneTransitionFinished(SceneTransitionDiff sceneTransitionDiff)
        {
            splashPresenter.PlaySplashAnimation();
        }
    }
}