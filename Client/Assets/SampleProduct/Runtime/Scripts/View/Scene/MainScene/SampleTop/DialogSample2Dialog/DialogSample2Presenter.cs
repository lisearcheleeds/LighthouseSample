using Cysharp.Threading.Tasks;
using LighthouseExtends.ScreenStack;
using SampleProduct.View.Scene.MainScene.SampleTop.DialogSample1Dialog;
using SampleProduct.View.Scene.MainScene.SampleTop.DialogSampleConfirmDialog;
using UnityEngine;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.SampleTop.DialogSample2Dialog
{
    public sealed class DialogSample2Presenter : IScreenStackPresenter
    {
        IScreenStackModule screenStackModule;

        DialogSample2View dialogView;
        DialogSample2Data screenStackData;

        [Inject]
        public void Construct(IScreenStackModule screenStackModule)
        {
            this.screenStackModule = screenStackModule;
        }

        public void Bind(DialogSample2View dialogView, DialogSample2Data screenStackData)
        {
            dialogView.SubscribeCloseButtonClick(OnClickCloseButton);
            dialogView.SubscribeOpenDialog1ButtonClick(OnClickOpenDialog1Button);
            dialogView.SubscribeOpenDialog2ButtonClick(OnClickOpenDialog2Button);
            dialogView.SubscribeConfirmOpenDialogButtonClick(OnClickOpenConfirmDialogButton);

            this.dialogView = dialogView;
            this.screenStackData = screenStackData;
        }

        UniTask IScreenStackPresenter.OnEnter(bool isResume)
        {
            dialogView.SetText($"Stack count {screenStackData.StackCount}");

            return UniTask.CompletedTask;
        }

        UniTask IScreenStackPresenter.OnLeave()
        {
            return UniTask.CompletedTask;
        }

        void OnClickCloseButton()
        {
            screenStackModule.Close().Forget();
        }

        void OnClickOpenDialog1Button()
        {
            screenStackModule.Open(new DialogSample1Data(screenStackData.StackCount + 1)).Forget();
        }

        void OnClickOpenDialog2Button()
        {
            screenStackModule.Open(new DialogSample2Data(screenStackData.StackCount + 1)).Forget();
        }

        void OnClickOpenConfirmDialogButton()
        {
            screenStackModule.Open(new DialogSampleConfirmData(() => screenStackModule.Close().Forget())).Forget();
        }
    }
}