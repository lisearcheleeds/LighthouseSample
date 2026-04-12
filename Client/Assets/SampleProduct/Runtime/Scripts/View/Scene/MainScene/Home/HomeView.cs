using System;
using LighthouseExtends.UIComponent.Button;
using SampleProduct.Extensions;
using UnityEngine;

namespace SampleProduct.View.Scene.MainScene.Home
{
    public sealed class HomeView : MonoBehaviour, IHomeView
    {
        [SerializeField] LHButton guideButton;
        [SerializeField] LHButton githubButton;
        [SerializeField] LHButton snsButton;

        [SerializeField] LHButton philosophyButton;
        [SerializeField] LHButton sampleButton;
        [SerializeField] LHButton requireToolsButton;

        [SerializeField] LHButton languageSwitchButton;

        IDisposable IHomeView.SubscribeGuideButtonClick(Action action) => guideButton.SubscribeOnClick(action);
        IDisposable IHomeView.SubscribeGithubButtonClick(Action action) => githubButton.SubscribeOnClick(action);
        IDisposable IHomeView.SubscribeSnsButtonClick(Action action) => snsButton.SubscribeOnClick(action);

        IDisposable IHomeView.SubscribePhilosophyButtonClick(Action action) => philosophyButton.SubscribeOnClick(action);
        IDisposable IHomeView.SubscribeSampleButtonClick(Action action) => sampleButton.SubscribeOnClick(action);
        IDisposable IHomeView.SubscribeRequireToolsButtonClick(Action action) => requireToolsButton.SubscribeOnClick(action);

        IDisposable IHomeView.SubscribeRequireLanguageSwitchButtonClick(Action action) => languageSwitchButton.SubscribeOnClick(action);
    }
}
