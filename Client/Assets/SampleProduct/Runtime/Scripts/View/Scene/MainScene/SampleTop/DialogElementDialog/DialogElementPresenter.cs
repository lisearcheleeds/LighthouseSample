using Cysharp.Threading.Tasks;
using LighthouseExtends.ScreenStack;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.SampleTop.DialogElementDialog
{
    public sealed class DialogElementPresenter
    {
        IScreenStackModule screenStackModule;

        [Inject]
        public void Construct(IScreenStackModule screenStackModule)
        {
            this.screenStackModule = screenStackModule;
        }

        public void Bind(DialogElementView view, DialogElementData data)
        {
            view.SubscribeCloseButtonClick(OnClickCloseButton);
        }

        void OnClickCloseButton()
        {
            screenStackModule.Close().Forget();
        }
    }
}
