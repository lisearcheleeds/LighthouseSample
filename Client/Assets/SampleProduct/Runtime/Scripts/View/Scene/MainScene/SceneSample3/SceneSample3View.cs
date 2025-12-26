using System;
using System.Collections.Generic;
using LighthouseExtends.TextTable;
using LighthouseExtends.UIComponent.Button;
using LighthouseExtends.TextMeshPro;
using SampleProduct.Extensions;
using UnityEngine;

namespace SampleProduct.View.Scene.MainScene.SceneSample3
{
    public class SceneSample3View : MonoBehaviour, ISceneSample3View
    {
        [SerializeField] LHButton backSceneButton;
        [SerializeField] LHTextMeshPro choiceDataText;

        IDisposable ISceneSample3View.SubscribeBackSceneButtonClick(Action action) => backSceneButton.SubscribeOnClick(action);
        bool ISceneSample3View.TryClickBackButton() => LHButtonHitChecker.TryClick(backSceneButton);

        void ISceneSample3View.ApplyChoiceData(int choiceData)
        {
            var param = new Dictionary<string, object>() { { "choiceData", choiceData } };
            choiceDataText.SetTextData(new TextData("SceneSample3ChoiceData", param));
        }
    }
}
