using System;
using LighthouseExtends.TextTable;
using LighthouseExtends.UIComponent.Button;
using LighthouseExtends.UIComponent.TextMeshPro;
using SampleProduct.Extensions;
using UnityEngine;

namespace SampleProduct.View.Scene.MainScene.SceneSample3
{
    public class SceneSample3View : MonoBehaviour, ISceneSample3View
    {
        [SerializeField] LHButton backSceneButton;
        [SerializeField] LHTextMeshPro choiceDataText;

        IDisposable ISceneSample3View.SubscribeBackSceneButtonClick(Action action) => backSceneButton.SubscribeOnClick(action);

        void ISceneSample3View.ApplyChoiceData(int choiceData)
        {
            choiceDataText.SetTextData(TextData.CreateTextData("SceneSample3", "ChoiceData", $"Choice: {choiceData}"));
        }
    }
}
