using Cysharp.Threading.Tasks;
using SampleProduct.Core;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.Purpose
{
    public sealed class PurposePresenter : IPurposePresenter
    {
        IPurposeView purposeView;
        IProductSceneManager sceneManager;

        [Inject]
        public void Constructor(
            IPurposeView purposeView,
            IProductSceneManager sceneManager)
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
