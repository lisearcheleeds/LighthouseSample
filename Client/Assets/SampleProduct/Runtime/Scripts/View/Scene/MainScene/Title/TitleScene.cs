using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
using SampleProduct.View.Base;
using VContainer;
using SampleProduct.LighthouseGenerated;

namespace SampleProduct.View.Scene.MainScene.Title
{
    public class TitleScene : ProductCanvasMainSceneBase<TitleScene.TitleTransitionData>
    {
        ITitlePresenter titlePresenter;

        public override MainSceneId MainSceneId => SampleProductMainSceneId.Title;

        public class TitleTransitionData : ProductTransitionDataBase
        {
            public override MainSceneId MainSceneId => SampleProductMainSceneId.Title;
        }

        [Inject]
        public void Constructor(ITitlePresenter titlePresenter)
        {
            this.titlePresenter = titlePresenter;
        }

        protected override UniTask OnSetup()
        {
            titlePresenter.Setup();
            return UniTask.CompletedTask;
        }
    }
}