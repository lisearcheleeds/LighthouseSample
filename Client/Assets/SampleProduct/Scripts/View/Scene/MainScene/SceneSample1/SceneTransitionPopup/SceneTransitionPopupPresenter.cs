using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
using LighthouseExtends.Popup;
using SampleProduct.View.Scene.MainScene.SceneSample2;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.SceneSample1.SceneTransitionPopup
{
    public sealed class SceneTransitionPopupPresenter : IPopupPresenter
    {
        IPopupModule popupModule;
        ISceneManager sceneManager;

        SceneTransitionPopupView popupView;
        SceneTransitionPopupData popupData;

        [Inject]
        public void Construct(IPopupModule popupModule, ISceneManager sceneManager)
        {
            this.popupModule = popupModule;
            this.sceneManager = sceneManager;
        }

        public void Bind(SceneTransitionPopupView popupView, SceneTransitionPopupData popupData)
        {
            popupView.SubscribeCloseButtonClick(OnClickCloseButton);
            popupView.SubscribeTransitionSceneButtonClick(OnTransitionSceneButtonClick);
            popupView.SubscribeTransitionSceneWithCloseButtonClick(OnTransitionSceneWithCloseButtonClick);

            this.popupView = popupView;
            this.popupData = popupData;
        }

        UniTask IPopupPresenter.OnEnter(bool isResume)
        {
            return UniTask.CompletedTask;
        }

        UniTask IPopupPresenter.OnLeave()
        {
            return UniTask.CompletedTask;
        }

        void OnClickCloseButton()
        {
            popupModule.ClosePopup(CancellationToken.None).Forget();
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
                await popupModule.ClosePopup(CancellationToken.None);
                sceneManager.TransitionScene(new SceneSample2Scene.SceneSample2TransitionData());
            }
        }
    }
}