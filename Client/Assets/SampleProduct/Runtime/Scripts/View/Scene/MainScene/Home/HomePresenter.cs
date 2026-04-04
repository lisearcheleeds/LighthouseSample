using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
using LighthouseExtends.ScreenStack;
using SampleProduct.View.Scene.MainScene.Home.RequireToolsDialog;
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
        IScreenStackModule screenStackModule;

        [Inject]
        public void Construct(
            IHomeView homeView,
            ISceneManager sceneManager,
            IBackgroundModule backgroundModule,
            IScreenStackModule screenStackModule)
        {
            this.homeView = homeView;
            this.sceneManager = sceneManager;
            this.backgroundModule = backgroundModule;
            this.screenStackModule = screenStackModule;
        }

        void IHomePresenter.Setup()
        {
            homeView.SubscribeGuideButtonClick(OnClickGuideButton);
            homeView.SubscribeGithubButtonClick(OnClickGithubButton);
            homeView.SubscribeSnsButtonClick(OnClickSnsButton);
            homeView.SubscribePhilosophyButtonClick(OnClickPhilosophyButton);
            homeView.SubscribeSampleButtonClick(OnClickSampleButton);
            homeView.SubscribeRequireToolsButtonClick(OnClickRequireToolsButton);
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
            // Cross fading is possible even if the scene groups are different, as long as the loading order is different. However, be mindful of performance.
            sceneManager.TransitionScene(new SampleTopScene.SampleTopTransitionData(), TransitionType.Cross);
        }

        void OnClickRequireToolsButton()
        {
            screenStackModule.Open(new RequireToolsData()).Forget();
        }
    }
}