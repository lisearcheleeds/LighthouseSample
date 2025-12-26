using System;
using LighthouseExtends.UIComponent.Button;
using SampleProduct.Extensions;
using UnityEngine;

namespace SampleProduct.View.Scene.MainScene.SceneSample2
{
    public class SceneSample2View : MonoBehaviour, ISceneSample2View
    {
        [SerializeField] LHButton choice1Button;
        [SerializeField] LHButton choice2Button;
        [SerializeField] LHButton choice3Button;
        [SerializeField] LHButton backSceneButton;

        IDisposable ISceneSample2View.SubscribeChoice1ButtonClick(Action action) => choice1Button.SubscribeOnClick(action);
        IDisposable ISceneSample2View.SubscribeChoice2ButtonClick(Action action) => choice2Button.SubscribeOnClick(action);
        IDisposable ISceneSample2View.SubscribeChoice3ButtonClick(Action action) => choice3Button.SubscribeOnClick(action);
        IDisposable ISceneSample2View.SubscribeBackSceneButtonClick(Action action) => backSceneButton.SubscribeOnClick(action);
        bool ISceneSample2View.TryClickBackButton() => LHButtonHitChecker.TryClick(backSceneButton);
    }
}