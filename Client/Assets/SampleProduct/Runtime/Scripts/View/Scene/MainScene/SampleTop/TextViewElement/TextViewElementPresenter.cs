using Cysharp.Threading.Tasks;
using LighthouseExtends.ScreenStack;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.SampleTop.TextViewElement
{
    public sealed class TextViewElementPresenter
    {
        IScreenStackModule screenStackModule;

        [Inject]
        public void Construct(IScreenStackModule screenStackModule)
        {
            this.screenStackModule = screenStackModule;
        }

        public void Bind(TextViewElementView view, TextViewElementData data)
        {
            view.SubscribeCloseButtonClick(OnClickCloseButton);
        }

        void OnClickCloseButton()
        {
            screenStackModule.Close().Forget();
        }
    }
}
