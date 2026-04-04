using System;
using LighthouseExtends.UIComponent.Button;
using SampleProduct.Extensions;
using UnityEngine;

namespace SampleProduct.View.Scene.MainScene.Title
{
    public class TitleView : MonoBehaviour, ITitleView
    {
        [SerializeField] LHButton screenButton;

        IDisposable ITitleView.SubscribeScreenButtonClick(Action action) => screenButton.SubscribeOnClick(action);
    }
}