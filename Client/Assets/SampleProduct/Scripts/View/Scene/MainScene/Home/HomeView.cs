using System;
using LighthouseExtends.UIComponent.Scripts.Button;
using SampleProduct.Extensions;
using UnityEngine;

namespace SampleProduct.View.Scene.MainScene.Home
{
    public sealed class HomeView : MonoBehaviour, IHomeView
    {
        [SerializeField] LHButton editButton;
        [SerializeField] LHButton game1Button;
        [SerializeField] LHButton game2Button;

        [SerializeField] LHButton sceneSample1Button;
        [SerializeField] LHButton sceneSample2Button;

        [SerializeField] LHButton popupSample1Button;
        [SerializeField] LHButton popupSample2Button;

        IDisposable IHomeView.SubscribeEditButtonClick(Action action) => editButton.SubscribeOnClick(action);
        IDisposable IHomeView.SubscribeGame1ButtonClick(Action action) => game1Button.SubscribeOnClick(action);
        IDisposable IHomeView.SubscribeGame2ButtonClick(Action action) => game2Button.SubscribeOnClick(action);

        IDisposable IHomeView.SubscribeSceneSample1ButtonClick(Action action) => sceneSample1Button.SubscribeOnClick(action);
        IDisposable IHomeView.SubscribeSceneSample2ButtonClick(Action action) => sceneSample2Button.SubscribeOnClick(action);

        IDisposable IHomeView.SubscribePopupSample1ButtonClick(Action action) => popupSample1Button.SubscribeOnClick(action);
        IDisposable IHomeView.SubscribePopupSample2ButtonClick(Action action) => popupSample2Button.SubscribeOnClick(action);
    }
}