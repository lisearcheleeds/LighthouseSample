using Cysharp.Threading.Tasks;
using LighthouseExtends.ScreenStack;
using VContainer;

namespace SampleProduct.PopupElement
{
    public sealed class PopupElementPresenter : IScreenStackPresenter
    {
        IScreenStackModule screenStackModule;

        PopupElementView dialogView;
        PopupElementData screenStackData;

        [Inject]
        public void Construct(IScreenStackModule screenStackModule)
        {
            this.screenStackModule = screenStackModule;
        }

        public void Bind(PopupElementView dialogView, PopupElementData screenStackData)
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
