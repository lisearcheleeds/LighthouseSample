using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
using LighthouseExtends.ScreenStack;
using SampleProduct.AnimationElement;
using SampleProduct.ButtonElement;
using SampleProduct.DialogElement;
using SampleProduct.OverlayElement;
using SampleProduct.PopupElement;
using SampleProduct.TextView;
using SampleProduct.TransitionAnimationElement;
using SampleProduct.View.Scene.MainScene.SampleTop.DialogSample1Dialog;
using SampleProduct.View.Scene.MainScene.SampleTop.DialogSample2Dialog;
using SampleProduct.View.Scene.MainScene.SceneSample1;
using SampleProduct.View.Scene.ModuleScene.Background;
using UnityEngine;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.SampleTop
{
    public sealed class SampleTopPresenter : ISampleTopPresenter
    {
        const int OverviewPageMax = 3;

        ISampleTopView sampleTopView;
        IScreenStackModule screenStackModule;
        ISceneManager sceneManager;
        IBackgroundModule backgroundModule;

        public TabType CurrentTabType { get; private set; }

        int overviewIndex;

        [Inject]
        public void Construct(
            ISampleTopView sampleTopView,
            IScreenStackModule screenStackModule,
            ISceneManager sceneManager,
            IBackgroundModule backgroundModule)
        {
            this.sampleTopView = sampleTopView;
            this.screenStackModule = screenStackModule;
            this.sceneManager = sceneManager;
            this.backgroundModule = backgroundModule;
        }

        void ISampleTopPresenter.Setup()
        {
            sampleTopView.SubscribeBackSceneButtonClick(OnClickBackScene);

            sampleTopView.SubscribeOverviewTabButtonClick(OnClickOverviewTab);
            sampleTopView.SubscribeSceneSampleTabButtonClick(OnClickSceneSampleTab);
            sampleTopView.SubscribeDialogSampleTabButtonClick(OnClickDialogSampleTab);
            sampleTopView.SubscribeElementTabButtonClick(OnClickElementTab);
            sampleTopView.SubscribeInGameTabButtonClick(OnClickInGameTab);

            sampleTopView.SubscribeOverviewNextPageButtonClick(OnClickOverviewNextPage);
            sampleTopView.SubscribeOverviewPrevPageButtonClick(OnClickOverviewPrevPage);
            sampleTopView.SubscribeOpenGitHubButtonClick(OnClickOpenGitHub);

            sampleTopView.SubscribeSceneSample1ButtonClick(OnClickSceneSample1);

            sampleTopView.SubscribeDialogSample1ButtonClick(OnClickDialogSample1);
            sampleTopView.SubscribeDialogSample2ButtonClick(OnClickDialogSample2);

            sampleTopView.SubscribeElementDialogButtonClick(OnClickElementDialog);
            sampleTopView.SubscribeElementPopupButtonClick(OnClickElementPopup);
            sampleTopView.SubscribeElementOverlayButtonClick(OnClickElementOverlay);
            sampleTopView.SubscribeAnimationButtonClick(OnClickAnimation);
            sampleTopView.SubscribeTransitionAnimationButtonClick(OnClickTransitionAnimation);
            sampleTopView.SubscribeButtonButtonClick(OnClickButton);
            sampleTopView.SubscribeTextButtonClick(OnClickText);

            sampleTopView.SubscribeGame1ButtonClick(OnClickGame1Button);
            sampleTopView.SubscribeGame2ButtonClick(OnClickGame2Button);
        }

        void ISampleTopPresenter.OnEnter(TabType targetTabType)
        {
            CurrentTabType = targetTabType;

            sampleTopView.ResetView();

            backgroundModule.SetBackgroundLayout(BackgroundLayout.SampleTop);
        }

        void ISampleTopPresenter.OnCompleteInAnimation()
        {
            sampleTopView.SwitchTab(TabType.None, CurrentTabType);
            sampleTopView.SetOverviewIndex(overviewIndex, overviewIndex);
        }

        void OnClickBackScene()
        {
            sceneManager.BackScene();
        }

        void OnClickOverviewTab()
        {
            if (CurrentTabType == TabType.Overview)
            {
                return;
            }

            sampleTopView.SwitchTab(CurrentTabType, TabType.Overview);
            CurrentTabType = TabType.Overview;
        }

        void OnClickSceneSampleTab()
        {
            if (CurrentTabType == TabType.SceneSample)
            {
                return;
            }

            sampleTopView.SwitchTab(CurrentTabType, TabType.SceneSample);
            CurrentTabType = TabType.SceneSample;
        }

        void OnClickDialogSampleTab()
        {
            if (CurrentTabType == TabType.DialogSample)
            {
                return;
            }

            sampleTopView.SwitchTab(CurrentTabType, TabType.DialogSample);
            CurrentTabType = TabType.DialogSample;
        }

        void OnClickElementTab()
        {
            if (CurrentTabType == TabType.Element)
            {
                return;
            }

            sampleTopView.SwitchTab(CurrentTabType, TabType.Element);
            CurrentTabType = TabType.Element;
        }

        void OnClickInGameTab()
        {
            if (CurrentTabType == TabType.InGame)
            {
                return;
            }

            sampleTopView.SwitchTab(CurrentTabType, TabType.InGame);
            CurrentTabType = TabType.InGame;
        }

        void OnClickOverviewNextPage()
        {
            var prevIndex = overviewIndex;
            overviewIndex = Mathf.Clamp(overviewIndex + 1, 0, OverviewPageMax);

            if (prevIndex == overviewIndex)
            {
                return;
            }

            sampleTopView.SetOverviewIndex(prevIndex, overviewIndex);
            sampleTopView.SetOverviewButtonState(overviewIndex != OverviewPageMax, overviewIndex != 0);
        }

        void OnClickOverviewPrevPage()
        {
            var prevIndex = overviewIndex;
            overviewIndex = Mathf.Clamp(overviewIndex - 1, 0, OverviewPageMax);

            if (prevIndex == overviewIndex)
            {
                return;
            }

            sampleTopView.SetOverviewIndex(prevIndex, overviewIndex);
            sampleTopView.SetOverviewButtonState(overviewIndex != OverviewPageMax, overviewIndex != 0);
        }

        void OnClickOpenGitHub()
        {
            Application.OpenURL("https://github.com/lisearcheleeds/LighthouseFramework/");
        }

        void OnClickSceneSample1()
        {
            sceneManager.TransitionScene(new SceneSample1Scene.SceneSample1TransitionData());
        }

        void OnClickDialogSample1()
        {
            screenStackModule.Open(new DialogSample1Data()).Forget();
        }

        void OnClickDialogSample2()
        {
            screenStackModule.Open(new DialogSample2Data()).Forget();
        }

        void OnClickElementDialog()
        {
            screenStackModule.Open(new DialogElementData()).Forget();
        }

        void OnClickElementPopup()
        {
            screenStackModule.Open(new PopupElementData()).Forget();
        }

        void OnClickElementOverlay()
        {
            screenStackModule.Open(new OverlayElementData()).Forget();
        }

        void OnClickAnimation()
        {
            screenStackModule.Open(new AnimationElementData()).Forget();
        }

        void OnClickTransitionAnimation()
        {
            screenStackModule.Open(new TransitionAnimationElementData()).Forget();
        }

        void OnClickButton()
        {
            screenStackModule.Open(new ButtonElementData()).Forget();
        }

        void OnClickText()
        {
            screenStackModule.Open(new TextViewElementData()).Forget();
        }

        void OnClickGame1Button()
        {
            Debug.Log("OnClickGame1Button");
        }

        void OnClickGame2Button()
        {
            Debug.Log("OnClickGame2Button");
        }
    }
}
