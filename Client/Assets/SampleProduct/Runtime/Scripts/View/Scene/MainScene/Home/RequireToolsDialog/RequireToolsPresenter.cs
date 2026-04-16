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
        PlayerInputActions playerInputActions;

        RequireToolsView dialogView;
        RequireToolsData screenStackData;
        IInputLayer inputLayer;

        [Inject]
        public void Construct(
            IScreenStackModule screenStackModule,
            IInputLayerController inputLayerController,
            PlayerInputActions playerInputActions)
        {
            this.screenStackModule = screenStackModule;
            this.inputLayerController = inputLayerController;
            this.playerInputActions = playerInputActions;
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
                inputLayer = new RequireToolsInputLayer(playerInputActions, OnClickCloseButton);
                inputLayerController.PushLayer(inputLayer, playerInputActions.Dialog);
            }
            return UniTask.CompletedTask;
        }

        UniTask IScreenStackPresenter.OnLeave()
        {
            if (inputLayer != null)
            {
                inputLayerController.PopLayer(inputLayer);
                inputLayer = null;
            }
            return UniTask.CompletedTask;
        }

        void OnClickCloseButton()
        {
            screenStackModule.Close().Forget();
        }
    }
}
