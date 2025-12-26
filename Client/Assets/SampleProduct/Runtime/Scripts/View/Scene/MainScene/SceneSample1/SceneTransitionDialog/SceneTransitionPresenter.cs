using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
using SampleProduct.Core;
using LighthouseExtends.ScreenStack;
using SampleProduct.View.Scene.MainScene.SceneSample2;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.SceneSample1.SceneTransitionDialog
{
    public sealed class SceneTransitionPresenter
    {
        IScreenStackModule screenStackModule;
        ISampleSceneManager sceneManager;

        SceneTransitionView view;

        [Inject]
        public void Construct(IScreenStackModule screenStackModule, ISampleSceneManager sceneManager)
        {
            this.screenStackModule = screenStackModule;
            this.sceneManager = sceneManager;
        }

        public void Bind(SceneTransitionView view, SceneTransitionData data)
        {
            this.view = view;
            view.SubscribeCloseButtonClick(OnClickCloseButton);
            view.SubscribeTransitionSceneButtonClick(OnTransitionSceneButtonClick);
            view.SubscribeTransitionSceneWithCloseButtonClick(OnTransitionSceneWithCloseButtonClick);
        }

        void OnClickCloseButton()
        {
            screenStackModule.Close().Forget();
        }

        void OnTransitionSceneButtonClick()
        {
            sceneManager.TransitionScene(new SceneSample2Scene.SceneSample2TransitionData()).Forget();
        }

        void OnTransitionSceneWithCloseButtonClick()
        {
            CloseAfterTransition().Forget();

            async UniTask CloseAfterTransition()
            {
                await screenStackModule.Close();
                await sceneManager.TransitionScene(new SceneSample2Scene.SceneSample2TransitionData());
            }
        }
    }
}
