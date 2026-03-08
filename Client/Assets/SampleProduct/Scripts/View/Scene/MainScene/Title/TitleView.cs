using System;
using LighthouseExtends.Animation;
using LighthouseExtends.UIComponent.Scripts.Button;
using SampleProduct.Extensions;
using UnityEngine;

namespace SampleProduct.View.Scene.MainScene.Title
{
    public class TitleView : MonoBehaviour, ITitleView
    {
        [SerializeField] LHButton screenButton;
        [SerializeField] LHAnimationClipPlayerComponent animationClipPlayer;

        IDisposable ITitleView.SubscribeScreenButtonClick(Action action) => screenButton.SubscribeOnClick(action);

        void ITitleView.PlayGoToHomeAnimation(Action action)
        {
            animationClipPlayer.PlayAnimation(true, true, action);
        }
    }
}