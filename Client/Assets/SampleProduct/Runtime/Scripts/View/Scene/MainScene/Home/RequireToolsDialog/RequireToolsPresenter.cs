using Cysharp.Threading.Tasks;
using LighthouseExtends.ScreenStack;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.Home.RequireToolsDialog
{
    public sealed class RequireToolsPresenter
    {
        IScreenStackModule screenStackModule;
        RequireToolsView dialogView;

        [Inject]
        public void Construct(IScreenStackModule screenStackModule)
        {
            this.screenStackModule = screenStackModule;
        }

        public void Bind(RequireToolsView dialogView, RequireToolsData screenStackData)
        {
            this.dialogView = dialogView;
            dialogView.SubscribeCloseButtonClick(OnClickCloseButton);
        }

        void OnClickCloseButton()
        {
            screenStackModule.Close().Forget();
        }
    }
}
