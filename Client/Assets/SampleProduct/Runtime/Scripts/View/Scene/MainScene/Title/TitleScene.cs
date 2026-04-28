using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
using SampleProduct.LighthouseGenerated;
using SampleProduct.View.Base;
using VContainer;

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
        public void Construct(ITitlePresenter titlePresenter)
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