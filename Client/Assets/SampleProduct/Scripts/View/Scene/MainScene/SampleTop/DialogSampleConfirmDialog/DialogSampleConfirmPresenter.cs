using Cysharp.Threading.Tasks;
using LighthouseExtends.ScreenStack;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.SampleTop.DialogSampleConfirmDialog
{
    public sealed class DialogSampleConfirmPresenter : IScreenStackPresenter
    {
        IScreenStackModule screenStackModule;
        DialogSampleConfirmData dialogData;

        [Inject]
        public void Construct(IScreenStackModule screenStackModule)
        {
            this.screenStackModule = screenStackModule;
        }

        public void Bind(DialogSampleConfirmView view, DialogSampleConfirmData dialogData)
        {
            this.dialogData = dialogData;

            view.SubscribeCloseButtonClick(OnClickCloseButton);
        }

        UniTask IScreenStackPresenter.OnEnter(bool isResume)
        {
            return UniTask.CompletedTask;
        }

        UniTask IScreenStackPresenter.OnLeave()
        {
            return UniTask.CompletedTask;
        }

        void OnClickCloseButton()
        {
            dialogData.OnCloseAction();
            screenStackModule.Close().Forget();
        }
    }
}