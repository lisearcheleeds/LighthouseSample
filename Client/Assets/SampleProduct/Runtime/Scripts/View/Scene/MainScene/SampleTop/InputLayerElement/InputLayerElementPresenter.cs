using Cysharp.Threading.Tasks;
using LighthouseExtends.ScreenStack;
using VContainer;

namespace SampleProduct.InputLayerElement
{
    public sealed class InputLayerElementPresenter
    {
        IScreenStackModule screenStackModule;

        InputLayerElementView dialogView;
        InputLayerElementData screenStackData;

        [Inject]
        public void Construct(IScreenStackModule screenStackModule)
        {
            this.screenStackModule = screenStackModule;
        }

        public void Bind(InputLayerElementView dialogView, InputLayerElementData screenStackData)
        {
            this.dialogView = dialogView;
            this.screenStackData = screenStackData;

            dialogView.SubscribeCloseButtonClick(OnClickCloseButton);
        }

        void OnClickCloseButton()
        {
            screenStackModule.Close().Forget();
        }
    }
}
