using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
using LighthouseExtends.Language;
using LighthouseExtends.ScreenStack;
using SampleProduct.Core;
using SampleProduct.View.Scene.MainScene.Home.RequireToolsDialog;
using SampleProduct.View.Scene.MainScene.Purpose;
using SampleProduct.View.Scene.MainScene.SampleTop;
using SampleProduct.View.Scene.ModuleScene.Background;
using UnityEngine;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.Home
{
    public sealed class HomePresenter : IHomePresenter
    {
        IHomeView homeView;
        IProductSceneManager sceneManager;
        IBackgroundModule backgroundModule;
        IScreenStackModule screenStackModule;
        ILanguageService languageService;
        ISupportedLanguageService supportedLanguageService;

        [Inject]
        public void Construct(
            IHomeView homeView,
            IProductSceneManager sceneManager,
            IBackgroundModule backgroundModule,
            IScreenStackModule screenStackModule,
            ILanguageService languageService,
            ISupportedLanguageService supportedLanguageService)
        {
            this.homeView = homeView;
            this.sceneManager = sceneManager;
            this.backgroundModule = backgroundModule;
            this.screenStackModule = screenStackModule;
            this.languageService = languageService;
            this.supportedLanguageService = supportedLanguageService;
        }

        void IHomePresenter.Setup()
        {
            homeView.SubscribeGuideButtonClick(OnClickGuideButton);
            homeView.SubscribeGithubButtonClick(OnClickGithubButton);
            homeView.SubscribeSnsButtonClick(OnClickSnsButton);
            homeView.SubscribePhilosophyButtonClick(OnClickPhilosophyButton);
            homeView.SubscribeSampleButtonClick(OnClickSampleButton);
            homeView.SubscribeRequireToolsButtonClick(OnClickRequireToolsButton);
            homeView.SubscribeRequireLanguageSwitchButtonClick(OnClickRequireLanguageSwitchButton);
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
            sceneManager.TransitionScene(new PurposeScene.PurposeTransitionData(), TransitionType.Cross).Forget();
        }

        void OnClickSampleButton()
        {
            // Cross fading is possible even if the scene groups are different, as long as the loading order is different. However, be mindful of performance.
            sceneManager.TransitionScene(new SampleTopScene.SampleTopTransitionData(), TransitionType.Cross).Forget();
        }

        void OnClickRequireToolsButton()
        {
            screenStackModule.Open(new RequireToolsData()).Forget();
        }

        void OnClickRequireLanguageSwitchButton()
        {
            var currentLanguageIndex = 0;
            for (var i = 0; i < supportedLanguageService.SupportedLanguages.Count; i++)
            {
                if (supportedLanguageService.SupportedLanguages[i] == languageService.CurrentLanguage.CurrentValue)
                {
                    currentLanguageIndex = i;
                    break;
                }
            }

            var nextLanguageIndex = (currentLanguageIndex + supportedLanguageService.SupportedLanguages.Count + 1) % supportedLanguageService.SupportedLanguages.Count;
            languageService.SetLanguage(supportedLanguageService.SupportedLanguages[nextLanguageIndex], CancellationToken.None).Forget();
        }
    }
}
