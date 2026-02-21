using System;
using LighthouseExtends.Button;
using SampleProduct.Extensions;
using UnityEngine;

namespace SampleProduct.View.Scene.MainScene.Home
{
    public sealed class HomeView : MonoBehaviour
    {
        [SerializeField] LHButton editButton;
        [SerializeField] LHButton optionButton;
        [SerializeField] LHButton dialogTestButton;

        public IDisposable SubscribeEditButtonClick(Action action) => editButton.SubscribeProjectDefault(action);
        public IDisposable SubscribeOptionButtonClick(Action action) => optionButton.SubscribeProjectDefault(action);
        public IDisposable SubscribeDialogTestButtonClick(Action action) => dialogTestButton.SubscribeProjectDefault(action);
    }
}