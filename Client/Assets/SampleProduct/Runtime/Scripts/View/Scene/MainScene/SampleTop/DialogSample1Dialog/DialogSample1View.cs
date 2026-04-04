using System;
using LighthouseExtends.UIComponent.Button;
using SampleProduct.Extensions;
using TMPro;
using UnityEngine;

namespace SampleProduct.View.Scene.MainScene.SampleTop.DialogSample1Dialog
{
    public class DialogSample1View : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI subtitleText;

        [SerializeField] LHButton closeButton;
        [SerializeField] LHButton openDialog1Button;
        [SerializeField] LHButton openDialog2Button;
        [SerializeField] LHButton openConfirmDialogButton;

        public IDisposable SubscribeCloseButtonClick(Action action) => closeButton.SubscribeOnClick(action);
        public IDisposable SubscribeOpenDialog1ButtonClick(Action action) => openDialog1Button.SubscribeOnClick(action);
        public IDisposable SubscribeOpenDialog2ButtonClick(Action action) => openDialog2Button.SubscribeOnClick(action);
        public IDisposable SubscribeConfirmOpenDialogButtonClick(Action action) => openConfirmDialogButton.SubscribeOnClick(action);

        public void SetText(string text)
        {
            subtitleText.text = text;
        }
    }
}