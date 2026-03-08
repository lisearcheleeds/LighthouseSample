using System;
using LighthouseExtends.UIComponent.Scripts.Button;
using SampleProduct.Extensions;
using UnityEngine;

namespace SampleProduct.View.Scene.MainScene.SceneSample2
{
    public class SceneSample2View : MonoBehaviour, ISceneSample2View
    {
        [SerializeField] LHButton transitionScene1Button;
        [SerializeField] LHButton transitionScene2Button;
        [SerializeField] LHButton backSceneButton;

        IDisposable ISceneSample2View.SubscribeTransitionScene1ButtonClick(Action action) => transitionScene1Button.SubscribeOnClick(action);
        IDisposable ISceneSample2View.SubscribeTransitionScene2ButtonClick(Action action) => transitionScene2Button.SubscribeOnClick(action);
        IDisposable ISceneSample2View.SubscribeBackSceneButtonClick(Action action) => backSceneButton.SubscribeOnClick(action);
    }
}