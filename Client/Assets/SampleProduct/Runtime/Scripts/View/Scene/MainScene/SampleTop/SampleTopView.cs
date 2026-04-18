using System;
using Cysharp.Threading.Tasks;
using LighthouseExtends.Animation.Runtime;
using LighthouseExtends.UIComponent.Button;
using SampleProduct.Extensions;
using UnityEngine;

namespace SampleProduct.View.Scene.MainScene.SampleTop
{
    public sealed class SampleTopView : MonoBehaviour, ISampleTopView
    {
        [SerializeField] LHButton backSceneButton;

        [SerializeField] LHButton overviewTabButton;
        [SerializeField] LHButton sceneSampleTabButton;
        [SerializeField] LHButton dialogSampleTabButton;
        [SerializeField] LHButton elementTabButton;
        [SerializeField] LHButton inGameTabButton;

        [SerializeField] LHButton overviewNextPageButton;
        [SerializeField] LHButton overviewPrevPageButton;
        [SerializeField] LHButton openGitHubButton;

        [SerializeField] LHButton sceneSample1Button;

        [SerializeField] LHButton dialogSample1Button;
        [SerializeField] LHButton dialogSample2Button;

        [SerializeField] LHButton elementDialogButton;
        [SerializeField] LHButton elementPopupButton;
        [SerializeField] LHButton elementOverlayButton;
        [SerializeField] LHButton animationButton;
        [SerializeField] LHButton transitionAnimationButton;
        [SerializeField] LHButton buttonButton;
        [SerializeField] LHButton textButton;

        [SerializeField] LHButton game1Button;
        [SerializeField] LHButton game2Button;

        [SerializeField] LHTransitionAnimator overViewTabButtonAnimator;
        [SerializeField] LHTransitionAnimator sceneSampleTabButtonAnimator;
        [SerializeField] LHTransitionAnimator dialogSampleTabButtonAnimator;
        [SerializeField] LHTransitionAnimator elementTabButtonAnimator;
        [SerializeField] LHTransitionAnimator inGameTabButtonAnimator;

        [SerializeField] LHTransitionAnimator overViewTabContentAnimator;
        [SerializeField] LHTransitionAnimator sceneSampleTabContentAnimator;
        [SerializeField] LHTransitionAnimator dialogSampleTabContentAnimator;
        [SerializeField] LHTransitionAnimator elementTabContentAnimator;
        [SerializeField] LHTransitionAnimator inGameTabContentAnimator;

        [SerializeField] LHTransitionAnimator[] overViewTabContentPageAnimators;

        IDisposable ISampleTopView.SubscribeBackSceneButtonClick(Action action) => backSceneButton.SubscribeOnClick(action);
        bool ISampleTopView.TryClickBackButton() => LHButtonHitChecker.TryClick(backSceneButton);

        IDisposable ISampleTopView.SubscribeOverviewTabButtonClick(Action action) => overviewTabButton.SubscribeOnClick(action);
        IDisposable ISampleTopView.SubscribeSceneSampleTabButtonClick(Action action) => sceneSampleTabButton.SubscribeOnClick(action);
        IDisposable ISampleTopView.SubscribeDialogSampleTabButtonClick(Action action) => dialogSampleTabButton.SubscribeOnClick(action);
        IDisposable ISampleTopView.SubscribeElementTabButtonClick(Action action) => elementTabButton.SubscribeOnClick(action);
        IDisposable ISampleTopView.SubscribeInGameTabButtonClick(Action action) => inGameTabButton.SubscribeOnClick(action);

        IDisposable ISampleTopView.SubscribeOverviewNextPageButtonClick(Action action) => overviewNextPageButton.SubscribeOnClick(action);
        IDisposable ISampleTopView.SubscribeOverviewPrevPageButtonClick(Action action) => overviewPrevPageButton.SubscribeOnClick(action);

        IDisposable ISampleTopView.SubscribeOpenGitHubButtonClick(Action action) => openGitHubButton.SubscribeOnClick(action);

        IDisposable ISampleTopView.SubscribeSceneSample1ButtonClick(Action action) => sceneSample1Button.SubscribeOnClick(action);

        IDisposable ISampleTopView.SubscribeDialogSample1ButtonClick(Action action) => dialogSample1Button.SubscribeOnClick(action);
        IDisposable ISampleTopView.SubscribeDialogSample2ButtonClick(Action action) => dialogSample2Button.SubscribeOnClick(action);

        IDisposable ISampleTopView.SubscribeElementDialogButtonClick(Action action) => elementDialogButton.SubscribeOnClick(action);
        IDisposable ISampleTopView.SubscribeElementPopupButtonClick(Action action) => elementPopupButton.SubscribeOnClick(action);
        IDisposable ISampleTopView.SubscribeElementOverlayButtonClick(Action action) => elementOverlayButton.SubscribeOnClick(action);
        IDisposable ISampleTopView.SubscribeAnimationButtonClick(Action action) => animationButton.SubscribeOnClick(action);
        IDisposable ISampleTopView.SubscribeTransitionAnimationButtonClick(Action action) => transitionAnimationButton.SubscribeOnClick(action);
        IDisposable ISampleTopView.SubscribeButtonButtonClick(Action action) => buttonButton.SubscribeOnClick(action);
        IDisposable ISampleTopView.SubscribeTextButtonClick(Action action) => textButton.SubscribeOnClick(action);

        IDisposable ISampleTopView.SubscribeGame1ButtonClick(Action action) => game1Button.SubscribeOnClick(action);
        IDisposable ISampleTopView.SubscribeGame2ButtonClick(Action action) => game2Button.SubscribeOnClick(action);

        void ISampleTopView.ResetView()
        {
            overViewTabButtonAnimator.EndOutAnimation();
            sceneSampleTabButtonAnimator.EndOutAnimation();
            dialogSampleTabButtonAnimator.EndOutAnimation();
            elementTabButtonAnimator.EndOutAnimation();
            inGameTabButtonAnimator.EndOutAnimation();

            overViewTabContentAnimator.EndOutAnimation();
            sceneSampleTabContentAnimator.EndOutAnimation();
            dialogSampleTabContentAnimator.EndOutAnimation();
            elementTabContentAnimator.EndOutAnimation();
            inGameTabContentAnimator.EndOutAnimation();

            foreach (var overViewTabContentPageAnimator in overViewTabContentPageAnimators)
            {
                overViewTabContentPageAnimator.EndOutAnimation();
            }

            overviewNextPageButton.interactable = true;
            overviewPrevPageButton.interactable = false;
        }

        void ISampleTopView.SwitchTab(TabType prevTabType,TabType tabType)
        {
            switch (prevTabType)
            {
                case TabType.Overview:
                    overViewTabButtonAnimator.OutAnimation().Forget();
                    overViewTabContentAnimator.OutAnimation().Forget();
                    break;
                case TabType.SceneSample:
                    sceneSampleTabButtonAnimator.OutAnimation().Forget();
                    sceneSampleTabContentAnimator.OutAnimation().Forget();
                    break;
                case TabType.DialogSample:
                    dialogSampleTabButtonAnimator.OutAnimation().Forget();
                    dialogSampleTabContentAnimator.OutAnimation().Forget();
                    break;
                case TabType.Element:
                    elementTabButtonAnimator.OutAnimation().Forget();
                    elementTabContentAnimator.OutAnimation().Forget();
                    break;
                case TabType.InGame:
                    inGameTabButtonAnimator.OutAnimation().Forget();
                    inGameTabContentAnimator.OutAnimation().Forget();
                    break;
                default:
                    break;
            }

            switch (tabType)
            {
                case TabType.Overview:
                    overViewTabButtonAnimator.InAnimation().Forget();
                    overViewTabContentAnimator.InAnimation().Forget();
                    break;
                case TabType.SceneSample:
                    sceneSampleTabButtonAnimator.InAnimation().Forget();
                    sceneSampleTabContentAnimator.InAnimation().Forget();
                    break;
                case TabType.DialogSample:
                    dialogSampleTabButtonAnimator.InAnimation().Forget();
                    dialogSampleTabContentAnimator.InAnimation().Forget();
                    break;
                case TabType.Element:
                    elementTabButtonAnimator.InAnimation().Forget();
                    elementTabContentAnimator.InAnimation().Forget();
                    break;
                case TabType.InGame:
                    inGameTabButtonAnimator.InAnimation().Forget();
                    inGameTabContentAnimator.InAnimation().Forget();
                    break;
                default:
                    break;
            }
        }

        void ISampleTopView.SetOverviewIndex(int prevIndex, int index)
        {
            overViewTabContentPageAnimators[prevIndex].OutAnimation().Forget();
            overViewTabContentPageAnimators[index].InAnimation().Forget();
        }

        void ISampleTopView.SetOverviewButtonState(bool isNextButtonActive, bool isPrevButtonActive)
        {
            overviewNextPageButton.interactable = isNextButtonActive;
            overviewPrevPageButton.interactable = isPrevButtonActive;
        }
    }
}
