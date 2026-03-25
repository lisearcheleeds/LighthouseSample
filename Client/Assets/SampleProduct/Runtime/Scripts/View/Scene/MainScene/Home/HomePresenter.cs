using Lighthouse.Scene;
using SampleProduct.View.Scene.MainScene.SampleTop;
using SampleProduct.View.Scene.ModuleScene.Background;
using UnityEngine;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.Home
{
    public sealed class HomePresenter : IHomePresenter
    {
        IHomeView homeView;
        ISceneManager sceneManager;
        IBackgroundModule backgroundModule;

        [Inject]
        public void Construct(
            IHomeView homeView,
            ISceneManager sceneManager,
            IBackgroundModule backgroundModule)
        {
            this.homeView = homeView;
            this.sceneManager = sceneManager;
            this.backgroundModule = backgroundModule;
        }

        void IHomePresenter.Setup()
        {
            homeView.SubscribeGuideButtonClick(OnClickGuideButton);
            homeView.SubscribeGithubButtonClick(OnClickGithubButton);
            homeView.SubscribeSnsButtonClick(OnClickSnsButton);
            homeView.SubscribePhilosophyButtonClick(OnClickPhilosophyButton);
            homeView.SubscribeSampleButtonClick(OnClickSampleButton);
            homeView.SubscribeElementsButtonClick(OnClickElementsButton);
        }

        void IHomePresenter.OnEnter()
        {
            backgroundModule.SetBackgroundLayout(BackgroundLayout.HomeLayout);
        }

        void OnClickGuideButton()
        {
            Application.OpenURL("https://github.com/lisearcheleeds/LighthouseFramework/");
        }

        void OnClickGithubButton()
        {
            Application.OpenURL("https://github.com/lisearcheleeds/LighthouseFramework/");
        }

        void OnClickSnsButton()
        {
            Application.OpenURL("https://x.com/archeleeds");
        }

        void OnClickPhilosophyButton()
        {
        }

        void OnClickSampleButton()
        {
            sceneManager.TransitionScene(new SampleTopScene.SampleTopTransitionData());
        }

        void OnClickElementsButton()
        {
        }
    }
}