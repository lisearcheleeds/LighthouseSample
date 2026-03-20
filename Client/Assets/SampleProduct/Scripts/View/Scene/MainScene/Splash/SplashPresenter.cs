using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
using SampleProduct.View.Scene.MainScene.Title;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.Splash
{
    public class SplashPresenter : ISplashPresenter
    {
        ISceneManager sceneManager;
        ISplashView splashView;

        [Inject]
        public void Construct(ISceneManager sceneManager, ISplashView splashView)
        {
            this.sceneManager = sceneManager;
            this.splashView = splashView;
        }

        void ISplashPresenter.PlaySplashAnimation()
        {
            UniTask.Void(async () =>
            {
                await splashView.PlaySplashAnimationAsync();
                await sceneManager.TransitionScene(new TitleScene.TitleTransitionData(), TransitionType.Exclusive);
            });
        }
    }
}