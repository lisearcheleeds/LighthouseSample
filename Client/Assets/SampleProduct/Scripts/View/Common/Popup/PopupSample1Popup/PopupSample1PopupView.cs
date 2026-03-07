using System;
using LighthouseExtends.UIComponent.Scripts.Button;
using SampleProduct.Extensions;
using TMPro;
using UnityEngine;

namespace SampleProduct.View.Common.Popup
{
    public class PopupSample1PopupView : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI subtitleText;

        [SerializeField] LHButton closeButton;
        [SerializeField] LHButton showCodeButton;
        [SerializeField] LHButton openPopup1Button;
        [SerializeField] LHButton openPopup2Button;
        [SerializeField] LHButton openConfirmPopupButton;

        public IDisposable SubscribeCloseButtonClick(Action action) => closeButton.SubscribeOnClick(action);
        public IDisposable SubscribeShowCodeButtonClick(Action action) => showCodeButton.SubscribeOnClick(action);
        public IDisposable SubscribeOpenPopup1ButtonClick(Action action) => openPopup1Button.SubscribeOnClick(action);
        public IDisposable SubscribeOpenPopup2ButtonClick(Action action) => openPopup2Button.SubscribeOnClick(action);
        public IDisposable SubscribeConfirmOpenPopupButtonClick(Action action) => openConfirmPopupButton.SubscribeOnClick(action);

        public void SetText(string text)
        {
            subtitleText.text = text;
        }
    }
}