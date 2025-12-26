using Cysharp.Threading.Tasks;
using LighthouseExtends.ScreenStack;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.SampleTop.DialogSampleConfirmDialog
{
    public sealed class DialogSampleConfirmPresenter
    {
        IScreenStackModule screenStackModule;
        DialogSampleConfirmData data;

        [Inject]
        public void Construct(IScreenStackModule screenStackModule)
        {
            this.screenStackModule = screenStackModule;
        }

        public void Bind(DialogSampleConfirmView view, DialogSampleConfirmData data)
        {
            this.data = data;
            view.SubscribeCloseButtonClick(OnClickCloseButton);
        }

        void OnClickCloseButton()
        {
            data.OnCloseAction();
            screenStackModule.Close().Forget();
        }
    }
}
