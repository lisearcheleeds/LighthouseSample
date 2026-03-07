using System;
using LighthouseExtends.UIComponent.Scripts.Button;
using SampleProduct.Extensions;
using UnityEngine;

namespace SampleProduct.View.Common.Popup
{
    public class PopupSampleConfirmPopupView : MonoBehaviour
    {
        [SerializeField] LHButton closeButton;

        public IDisposable SubscribeCloseButtonClick(Action action) => closeButton.SubscribeOnClick(action);
    }
}