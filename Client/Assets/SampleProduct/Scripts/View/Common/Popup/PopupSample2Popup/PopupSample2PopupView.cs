using System;
using LighthouseExtends.UI.Button;
using SampleProduct.Extensions;
using UnityEngine;

namespace SampleProduct.View.Common.Popup.PopupTest
{
    public class PopupSample2PopupView : MonoBehaviour
    {
        [SerializeField] LHButton closeButton;
        [SerializeField] LHButton showCodeButton;
        [SerializeField] LHButton openPopup1Button;
        [SerializeField] LHButton openPopup2Button;

        public IDisposable SubscribeCloseButtonClick(Action action) => closeButton.SubscribeOnClick(action);
        public IDisposable SubscribeShowCodeButtonClick(Action action) => showCodeButton.SubscribeOnClick(action);
        public IDisposable SubscribeOpenPopup1ButtonClick(Action action) => openPopup1Button.SubscribeOnClick(action);
        public IDisposable SubscribeOpenPopup2ButtonClick(Action action) => openPopup2Button.SubscribeOnClick(action);
    }
}