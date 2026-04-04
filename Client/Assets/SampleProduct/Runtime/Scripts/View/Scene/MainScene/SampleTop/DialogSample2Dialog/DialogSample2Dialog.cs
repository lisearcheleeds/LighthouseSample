using LighthouseExtends.ScreenStack;
using SampleProduct.View.Base;
using UnityEngine;

namespace SampleProduct.View.Scene.MainScene.SampleTop.DialogSample2Dialog
{
    public sealed class DialogSample2Dialog : StandardDialogBase, IScreenStackSetup<DialogSample2Presenter, DialogSample2Data>
    {
        [SerializeField] DialogSample2View dialogSample2View;

        public void Setup(DialogSample2Presenter dialogPresenter, DialogSample2Data screenStackData)
        {
            dialogPresenter.Bind(dialogSample2View, screenStackData);
        }
    }
}