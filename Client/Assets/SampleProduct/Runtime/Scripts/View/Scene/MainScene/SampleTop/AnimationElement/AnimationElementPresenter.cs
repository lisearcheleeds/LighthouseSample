using Cysharp.Threading.Tasks;
using LighthouseExtends.ScreenStack;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.SampleTop.AnimationElement
{
    public sealed class AnimationElementPresenter : IScreenStackPresenter
    {
        IScreenStackModule screenStackModule;

        AnimationElementView dialogView;
        AnimationElementData screenStackData;

        [Inject]
        public void Construct(IScreenStackModule screenStackModule)
        {
            this.screenStackModule = screenStackModule;
        }

        public void Bind(AnimationElementView dialogView, AnimationElementData screenStackData)
        {
            this.dialogView = dialogView;
            this.screenStackData = screenStackData;

            dialogView.SubscribeCloseButtonClick(OnClickCloseButton);
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
