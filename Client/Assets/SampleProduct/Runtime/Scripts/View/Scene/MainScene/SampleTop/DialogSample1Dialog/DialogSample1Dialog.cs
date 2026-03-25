using LighthouseExtends.ScreenStack;
using SampleProduct.View.Base;
using UnityEngine;

namespace SampleProduct.View.Scene.MainScene.SampleTop.DialogSample1Dialog
{
    public sealed class DialogSample1Dialog : StandardDialogBase, IScreenStackSetup<DialogSample1Presenter, DialogSample1Data>
    {
        [SerializeField] DialogSample1View dialogSample1View;

        public void Setup(DialogSample1Presenter dialogPresenter, DialogSample1Data screenStackData)
        {
            dialogPresenter.Bind(dialogSample1View, screenStackData);
        }
    }
}