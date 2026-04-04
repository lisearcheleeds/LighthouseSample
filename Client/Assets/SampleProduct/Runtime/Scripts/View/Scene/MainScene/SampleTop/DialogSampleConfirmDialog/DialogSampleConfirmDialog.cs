using LighthouseExtends.ScreenStack;
using SampleProduct.View.Base;
using UnityEngine;

namespace SampleProduct.View.Scene.MainScene.SampleTop.DialogSampleConfirmDialog
{
    public sealed class DialogSampleConfirmDialog : StandardDialogBase, IScreenStackSetup<DialogSampleConfirmPresenter, DialogSampleConfirmData>
    {
        [SerializeField] DialogSampleConfirmView dialogSampleConfirmView;

        public void Setup(DialogSampleConfirmPresenter dialogPresenter, DialogSampleConfirmData dialogData)
        {
            dialogPresenter.Bind(dialogSampleConfirmView, dialogData);
        }
    }
}