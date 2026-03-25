using System;
using LighthouseExtends.UIComponent.Button;
using SampleProduct.Extensions;
using TMPro;
using UnityEngine;

namespace SampleProduct.View.Scene.MainScene.SceneSample3
{
    public class SceneSample3View : MonoBehaviour, ISceneSample3View
    {
        [SerializeField] LHButton backSceneButton;
        [SerializeField] TextMeshProUGUI choiceDataText;

        IDisposable ISceneSample3View.SubscribeBackSceneButtonClick(Action action) => backSceneButton.SubscribeOnClick(action);

        void ISceneSample3View.ApplyChoiceData(int choiceData)
        {
            choiceDataText.text = $"Choice: {choiceData}";
        }
    }
}