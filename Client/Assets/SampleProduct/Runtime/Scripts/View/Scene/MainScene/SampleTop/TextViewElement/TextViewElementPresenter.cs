using Cysharp.Threading.Tasks;
using LighthouseExtends.ScreenStack;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.SampleTop.TextViewElement
{
    public sealed class TextViewElementPresenter : IScreenStackPresenter
    {
        IScreenStackModule screenStackModule;

        TextViewElementView textViewElementView;
        TextViewElementData screenStackElementData;

        [Inject]
        public void Construct(IScreenStackModule screenStackModule)
        {
            this.screenStackModule = screenStackModule;
        }

        public void Bind(TextViewElementView textViewElementView, TextViewElementData screenStackElementData)
        {
            this.textViewElementView = textViewElementView;
            this.screenStackElementData = screenStackElementData;

            textViewElementView.SubscribeCloseButtonClick(OnClickCloseButton);
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
