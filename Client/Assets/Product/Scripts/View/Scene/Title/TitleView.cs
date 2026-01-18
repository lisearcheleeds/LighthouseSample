using System;
using Product.View.Animation;
using UnityEngine;
using UnityEngine.UI;

namespace Product.View.Scene.Title
{
    public class TitleView : MonoBehaviour
    {
        [SerializeField] Button screenButton;
        [SerializeField] AnimatorTrigger animatorTrigger;

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
            animatorTrigger.SetTrigger(AnimatorKey.Play, AnimatorKey.EndState, onComplete: onClickScreenButtonAction);
        }
    }
}