using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
using SampleProduct.Core;
using SampleProduct.View.Scene.MainScene.Title;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.Splash
{
    public class SplashPresenter : ISplashPresenter
    {
        ISampleSceneManager sceneManager;

        [Inject]
        public void Construct(ISampleSceneManager sceneManager)
        {
            this.sceneManager = sceneManager;
        }

        void ISplashPresenter.PlaySplashAnimation()
        {
            UniTask.Void(async () =>
            {
                await UniTask.Delay(1500);
                await sceneManager.TransitionScene(new TitleScene.TitleTransitionData(), TransitionType.Exclusive);
            });
        }
    }
}