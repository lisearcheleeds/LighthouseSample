using Cysharp.Threading.Tasks;
using LighthouseExtends.InputLayer;
using LighthouseExtends.ScreenStack;
using SampleProduct.Input;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.Home.RequireToolsDialog
{
    public sealed class RequireToolsPresenter : IScreenStackPresenter
    {
        IScreenStackModule screenStackModule;
        IInputLayerController inputLayerController;

        RequireToolsView dialogView;
        RequireToolsData screenStackData;
        InputLayer inputLayer;

        [Inject]
        public void Construct(
            IScreenStackModule screenStackModule,
            IInputLayerController inputLayerController)
        {
            this.screenStackModule = screenStackModule;
            this.inputLayerController = inputLayerController;
        }

        public void Bind(RequireToolsView dialogView, RequireToolsData screenStackData)
        {
            dialogView.SubscribeCloseButtonClick(OnClickCloseButton);

            this.dialogView = dialogView;
            this.screenStackData = screenStackData;
        }

        UniTask IScreenStackPresenter.OnEnter(bool isResume)
        {
            if (!isResume)
            {
                inputLayer = new RequireToolsInputLayer(OnClickCloseButton);
                inputLayerController.PushLayer(inputLayer);
            }
            return UniTask.CompletedTask;
        }

        UniTask IScreenStackPresenter.OnLeave()
        {
            inputLayerController.PopLayer(inputLayer);
            inputLayer = null;
            return UniTask.CompletedTask;
        }

        void OnClickCloseButton()
        {
            screenStackModule.Close().Forget();
        }
    }
}
