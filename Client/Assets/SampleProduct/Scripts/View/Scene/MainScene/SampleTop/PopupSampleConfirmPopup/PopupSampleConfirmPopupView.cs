using System;
using LighthouseExtends.UIComponent.Scripts.Button;
using SampleProduct.Extensions;
using UnityEngine;

namespace SampleProduct.View.Scene.MainScene.SampleTop.PopupSampleConfirmPopup
{
    public class PopupSampleConfirmPopupView : MonoBehaviour
    {
        [SerializeField] LHButton closeButton;

        public IDisposable SubscribeCloseButtonClick(Action action) => closeButton.SubscribeOnClick(action);
    }
}