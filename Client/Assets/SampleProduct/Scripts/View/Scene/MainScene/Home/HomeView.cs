using System;
using LighthouseExtends.UI.Button;
using SampleProduct.Extensions;
using UnityEngine;

namespace SampleProduct.View.Scene.MainScene.Home
{
    public sealed class HomeView : MonoBehaviour, IHomeView
    {
        [SerializeField] LHButton editButton;
        [SerializeField] LHButton game1Button;
        [SerializeField] LHButton game2Button;

        [SerializeField] LHButton optionButton;

        [SerializeField] LHButton popupTest1Button;
        [SerializeField] LHButton popupTest2Button;

        public IDisposable SubscribeEditButtonButtonClick(Action action) => editButton.SubscribeOnClick(action);
        public IDisposable SubscribeGame1ButtonButtonClick(Action action) => game1Button.SubscribeOnClick(action);
        public IDisposable SubscribeGame2ButtonButtonClick(Action action) => game2Button.SubscribeOnClick(action);

        public IDisposable SubscribeOptionButtonClick(Action action) => optionButton.SubscribeOnClick(action);

        public IDisposable SubscribePopupTest1ButtonClick(Action action) => popupTest1Button.SubscribeOnClick(action);
        public IDisposable SubscribePopupTest2ButtonClick(Action action) => popupTest2Button.SubscribeOnClick(action);
    }
}