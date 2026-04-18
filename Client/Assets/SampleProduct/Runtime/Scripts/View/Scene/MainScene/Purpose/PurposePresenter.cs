using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.Purpose
{
    public sealed class PurposePresenter : IPurposePresenter
    {
        IPurposeView purposeView;
        ISceneManager sceneManager;

        [Inject]
        public void Construct(
            IPurposeView purposeView,
            ISceneManager sceneManager)
        {
            this.purposeView = purposeView;
            this.sceneManager = sceneManager;
        }

        void IPurposePresenter.Setup()
        {
            purposeView.SubscribeBackSceneButtonClick(OnClickBackScene);
        }

        void IPurposePresenter.OnEnter()
        {
        }

        bool IPurposePresenter.TryClickBackButton() => purposeView.TryClickBackButton();

        void OnClickBackScene()
        {
            sceneManager.BackScene().Forget();
        }
    }
}
