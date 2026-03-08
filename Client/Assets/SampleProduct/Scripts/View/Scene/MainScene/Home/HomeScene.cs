using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
using SampleProduct.View.Base;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.Home
{
    public class HomeScene : ProductCanvasMainSceneBase<HomeScene.HomeTransitionData>
    {
        IHomePresenter homePresenter;

        public override MainSceneId MainSceneId => SampleProductMainSceneId.Home;

        public class HomeTransitionData : ProductTransitionDataBase
        {
            public override MainSceneId MainSceneId => SampleProductMainSceneId.Home;
        }

        [Inject]
        public void Constructor(IHomePresenter homePresenter)
        {
            this.homePresenter = homePresenter;
        }

        protected override UniTask OnSetup()
        {
            homePresenter.Setup();
            return UniTask.CompletedTask;
        }
    }
}