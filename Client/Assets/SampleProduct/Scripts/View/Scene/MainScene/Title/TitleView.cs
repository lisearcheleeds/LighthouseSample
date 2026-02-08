using System;
using Lighthouse.Extends.Animation;
using Lighthouse.Extends.UI;
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