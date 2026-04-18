using System;
using LighthouseExtends.UIComponent.Button;
using SampleProduct.Extensions;
using UnityEngine;

namespace SampleProduct.View.Scene.MainScene.Home.RequireToolsDialog
{
    public class RequireToolsView : MonoBehaviour
    {
        [SerializeField] LHButton closeButton;

        public IDisposable SubscribeCloseButtonClick(Action action) => closeButton.SubscribeOnClick(action);
        public bool TryClickCloseButton() => LHButtonHitChecker.TryClick(closeButton);
    }
}