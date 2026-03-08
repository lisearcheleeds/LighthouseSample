using Lighthouse.Scene;
using SampleProduct.View.Scene.MainScene.Home;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.Title
{
    public class TitlePresenter : ITitlePresenter
    {
        ISceneManager sceneManager;
        ITitleView homeView;

        [Inject]
        public void Construct(ISceneManager sceneManager, ITitleView homeView)
        {
            this.sceneManager = sceneManager;
            this.homeView = homeView;
        }

        void ITitlePresenter.Setup()
        {
            homeView.SubscribeScreenButtonClick(OnClickScreenButton);
        }

        void OnClickScreenButton()
        {
            homeView.PlayGoToHomeAnimation(() =>
            {
                sceneManager.TransitionScene(new HomeScene.HomeTransitionData());
            });
        }
    }
}