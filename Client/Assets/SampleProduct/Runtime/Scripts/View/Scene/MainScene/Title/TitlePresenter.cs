using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
using SampleProduct.View.Scene.MainScene.Home;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.Title
{
    public class TitlePresenter : ITitlePresenter
    {
        ISceneManager sceneManager;
        ITitleView titleView;

        [Inject]
        public void Construct(ISceneManager sceneManager, ITitleView titleView)
        {
            this.sceneManager = sceneManager;
            this.titleView = titleView;
        }

        void ITitlePresenter.Setup()
        {
            titleView.SubscribeScreenButtonClick(OnClickScreenButton);
        }

        void OnClickScreenButton()
        {
            sceneManager.TransitionScene(new HomeScene.HomeTransitionData()).Forget();
        }
    }
}