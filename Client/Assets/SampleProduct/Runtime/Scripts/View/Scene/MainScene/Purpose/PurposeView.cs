using System;
using LighthouseExtends.UIComponent.Button;
using SampleProduct.Extensions;
using UnityEngine;

namespace SampleProduct.View.Scene.MainScene.Purpose
{
    public sealed class PurposeView : MonoBehaviour, IPurposeView
    {
        [SerializeField] LHButton backSceneButton;

        IDisposable IPurposeView.SubscribeBackSceneButtonClick(Action action) => backSceneButton.SubscribeOnClick(action);
    }
}
