using Cysharp.Threading.Tasks;
using LighthouseExtends.ScreenStack;
using SampleProduct.View.Scene.MainScene.SampleTop.DialogSample2Dialog;
using SampleProduct.View.Scene.MainScene.SampleTop.DialogSampleConfirmDialog;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.SampleTop.DialogSample1Dialog
{
    public sealed class DialogSample1Presenter
    {
        IScreenStackModule screenStackModule;

        DialogSample1View view;
        DialogSample1Data data;

        [Inject]
        public void Construct(IScreenStackModule screenStackModule)
        {
            this.screenStackModule = screenStackModule;
        }

        public void Bind(DialogSample1View view, DialogSample1Data data)
        {
            this.view = view;
            this.data = data;
            view.SubscribeCloseButtonClick(OnClickCloseButton);
            view.SubscribeOpenDialog1ButtonClick(OnClickOpenDialog1Button);
            view.SubscribeOpenDialog2ButtonClick(OnClickOpenDialog2Button);
            view.SubscribeConfirmOpenDialogButtonClick(OnClickOpenConfirmDialogButton);
        }

        public UniTask OnEnter(bool isResume)
        {
            view.SetText($"Stack count {data.StackCount}");
            return UniTask.CompletedTask;
        }

        void OnClickCloseButton()
        {
            screenStackModule.Close().Forget();
        }

        void OnClickOpenDialog1Button()
        {
            screenStackModule.Open(new DialogSample1Data(data.StackCount + 1)).Forget();
        }

        void OnClickOpenDialog2Button()
        {
            screenStackModule.Open(new DialogSample2Data(data.StackCount + 1)).Forget();
        }

        void OnClickOpenConfirmDialogButton()
        {
            screenStackModule.Open(new DialogSampleConfirmData(() => screenStackModule.Close().Forget())).Forget();
        }
    }
}
