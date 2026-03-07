using System;
using LighthouseExtends.Animation;
using LighthouseExtends.UIComponent.Scripts.Button;
using UnityEngine;

namespace SampleProduct.View.Scene.MainScene.Title
{
    public class TitleView : MonoBehaviour
    {
        [SerializeField] LHButton screenButton;
        [SerializeField] LHAnimationClipPlayerComponent animationClipPlayer;

        Action onClickScreenButtonAction;

        void Awake()
        {
            screenButton.onClick.AddListener(OnClickScreenButton);
        }

        public void Setup(Action onClickScreenButtonAction)
        {
            this.onClickScreenButtonAction = onClickScreenButtonAction;
        }

        void OnClickScreenButton()
        {
            animationClipPlayer.PlayAnimation(true, true, onClickScreenButtonAction);
        }
    }
}