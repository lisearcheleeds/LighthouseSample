using System;
using LighthouseExtends.UIComponent.Button;
using SampleProduct.Extensions;
using UnityEngine;

namespace SampleProduct.View.Scene.MainScene.SceneSample1.SceneTransitionDialog
{
    public class SceneTransitionView : MonoBehaviour
    {
        [SerializeField] LHButton closeButton;
        [SerializeField] LHButton transitionSceneButton;
        [SerializeField] LHButton transitionSceneWithCloseButton;

        public IDisposable SubscribeCloseButtonClick(Action action) => closeButton.SubscribeOnClick(action);
        public IDisposable SubscribeTransitionSceneButtonClick(Action action) => transitionSceneButton.SubscribeOnClick(action);
        public IDisposable SubscribeTransitionSceneWithCloseButtonClick(Action action) => transitionSceneWithCloseButton.SubscribeOnClick(action);
    }
}