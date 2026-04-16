using Cysharp.Threading.Tasks;
using LighthouseExtends.ScreenStack;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.SampleTop.OverlayElement
{
    public sealed class OverlayElementPresenter : IScreenStackPresenter
    {
        IScreenStackModule screenStackModule;

        OverlayElementView dialogView;
        OverlayElementData screenStackData;

        [Inject]
        public void Construct(IScreenStackModule screenStackModule)
        {
            this.screenStackModule = screenStackModule;
        }

        public void Bind(OverlayElementView dialogView, OverlayElementData screenStackData)
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
