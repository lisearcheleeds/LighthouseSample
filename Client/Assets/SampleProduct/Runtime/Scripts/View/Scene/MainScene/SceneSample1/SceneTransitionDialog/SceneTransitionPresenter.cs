using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
using LighthouseExtends.ScreenStack;
using SampleProduct.View.Scene.MainScene.SceneSample2;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.SceneSample1.SceneTransitionDialog
{
    public sealed class SceneTransitionPresenter : IScreenStackPresenter
    {
        IScreenStackModule screenStackModule;
        ISceneManager sceneManager;

        SceneTransitionView view;
        SceneTransitionData data;

        [Inject]
        public void Construct(IScreenStackModule screenStackModule, ISceneManager sceneManager)
        {
            this.screenStackModule = screenStackModule;
            this.sceneManager = sceneManager;
        }

        public void Bind(SceneTransitionView view, SceneTransitionData data)
        {
            view.SubscribeCloseButtonClick(OnClickCloseButton);
            view.SubscribeTransitionSceneButtonClick(OnTransitionSceneButtonClick);
            view.SubscribeTransitionSceneWithCloseButtonClick(OnTransitionSceneWithCloseButtonClick);

            this.view = view;
            this.data = data;
        }

        UniTask IScreenStackPresenter.OnEnter(bool isResume)
        {
            return UniTask.CompletedTask;
        }

        UniTask IScreenStackPresenter.OnLeave()
        {
            return UniTask.CompletedTask;
        }

        void OnClickCloseButton()
        {
            screenStackModule.Close().Forget();
        }

        void OnTransitionSceneButtonClick()
        {
            sceneManager.TransitionScene(new SceneSample2Scene.SceneSample2TransitionData());
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