using Cysharp.Threading.Tasks;
using LighthouseExtends.ScreenStack;
using VContainer;

namespace SampleProduct.DialogElement
{
    public sealed class DialogElementPresenter : IScreenStackPresenter
    {
        IScreenStackModule screenStackModule;

        DialogElementView dialogView;
        DialogElementData screenStackData;

        [Inject]
        public void Construct(IScreenStackModule screenStackModule)
        {
            this.screenStackModule = screenStackModule;
        }

        public void Bind(DialogElementView dialogView, DialogElementData screenStackData)
        {
            this.dialogView = dialogView;
            this.screenStackData = screenStackData;

            dialogView.SubscribeCloseButtonClick(OnClickCloseButton);
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
            screenStackModule.Close().Forget();
        }
    }
}
