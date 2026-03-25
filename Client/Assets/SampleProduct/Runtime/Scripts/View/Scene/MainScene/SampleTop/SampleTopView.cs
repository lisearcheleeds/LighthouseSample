using System;
using LighthouseExtends.UIComponent.Button;
using SampleProduct.Extensions;
using UnityEngine;

namespace SampleProduct.View.Scene.MainScene.SampleTop
{
    public sealed class SampleTopView : MonoBehaviour, ISampleTopView
    {
        [SerializeField] LHButton backSceneButton;

        [SerializeField] LHButton editButton;
        [SerializeField] LHButton game1Button;
        [SerializeField] LHButton game2Button;

        [SerializeField] LHButton sceneSample1Button;

        [SerializeField] LHButton dialogSample1Button;
        [SerializeField] LHButton dialogSample2Button;

        IDisposable ISampleTopView.SubscribeBackSceneButtonClick(Action action) => backSceneButton.SubscribeOnClick(action);

        IDisposable ISampleTopView.SubscribeEditButtonClick(Action action) => editButton.SubscribeOnClick(action);
        IDisposable ISampleTopView.SubscribeGame1ButtonClick(Action action) => game1Button.SubscribeOnClick(action);
        IDisposable ISampleTopView.SubscribeGame2ButtonClick(Action action) => game2Button.SubscribeOnClick(action);

        IDisposable ISampleTopView.SubscribeSceneSample1ButtonClick(Action action) => sceneSample1Button.SubscribeOnClick(action);

        IDisposable ISampleTopView.SubscribeDialogSample1ButtonClick(Action action) => dialogSample1Button.SubscribeOnClick(action);
        IDisposable ISampleTopView.SubscribeDialogSample2ButtonClick(Action action) => dialogSample2Button.SubscribeOnClick(action);
    }
}