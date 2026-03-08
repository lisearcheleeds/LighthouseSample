using System;
using LighthouseExtends.UIComponent.Scripts.Button;
using SampleProduct.Extensions;
using UnityEngine;

namespace SampleProduct.View.Scene.MainScene.SceneSample1
{
    public class SceneSample1View : MonoBehaviour, ISceneSample1View
    {
        [SerializeField] LHButton transitionScene1Button;
        [SerializeField] LHButton transitionScene2Button;
        [SerializeField] LHButton backSceneButton;

        IDisposable ISceneSample1View.SubscribeTransitionScene1ButtonClick(Action action) => transitionScene1Button.SubscribeOnClick(action);
        IDisposable ISceneSample1View.SubscribeTransitionScene2ButtonClick(Action action) => transitionScene2Button.SubscribeOnClick(action);
        IDisposable ISceneSample1View.SubscribeBackSceneButtonClick(Action action) => backSceneButton.SubscribeOnClick(action);
    }
}