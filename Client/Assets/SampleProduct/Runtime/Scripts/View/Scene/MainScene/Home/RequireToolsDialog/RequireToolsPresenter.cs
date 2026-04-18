using Cysharp.Threading.Tasks;
using LighthouseExtends.InputLayer;
using LighthouseExtends.ScreenStack;
using SampleProduct.Input;
using SampleProduct.Input.Layer;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.Home.RequireToolsDialog
{
    public sealed class RequireToolsPresenter : IScreenStackPresenter
    {
        IScreenStackModule screenStackModule;
        IInputLayerController inputLayerController;
        InputActions inputActions;

        RequireToolsView dialogView;
        RequireToolsData screenStackData;
        IInputLayer inputLayer;

        [Inject]
        public void Construct(
            IScreenStackModule screenStackModule,
            IInputLayerController inputLayerController,
            InputActions inputActions)
        {
            this.screenStackModule = screenStackModule;
            this.inputLayerController = inputLayerController;
            this.inputActions = inputActions;
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
                inputLayer = new DefaultScreenStackInputLayer(inputActions, OnClickCloseButton);
                inputLayerController.PushLayer(inputLayer, inputActions.Dialog);
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
