using Cysharp.Threading.Tasks;
using LighthouseExtends.ScreenStack;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.SampleTop.PopupElement
{
    public sealed class PopupElementPresenter
    {
        IScreenStackModule screenStackModule;

        [Inject]
        public void Construct(IScreenStackModule screenStackModule)
        {
            this.screenStackModule = screenStackModule;
        }

        public void Bind(PopupElementView view, PopupElementData data)
        {
            view.SubscribeCloseButtonClick(OnClickCloseButton);
        }

        void OnClickCloseButton()
        {
            screenStackModule.Close().Forget();
        }
    }
}
