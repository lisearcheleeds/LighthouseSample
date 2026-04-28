using Cysharp.Threading.Tasks;
using SampleProduct.Core;
using SampleProduct.View.Scene.MainScene.Home;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.Title
{
    public class TitlePresenter : ITitlePresenter
    {
        IProductSceneManager sceneManager;
        ITitleView titleView;

        [Inject]
        public void Construct(IProductSceneManager sceneManager, ITitleView titleView)
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
