using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
using SampleProduct.View.Scene.Common;
using SampleProduct.View.Scene.MainScene.Home;
using UnityEngine;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.Title
{
    public class TitleScene : ProductCanvasMainSceneBase<TitleScene.TitleTransitionData>
    {
        ISceneManager sceneManager;

        [SerializeField] TitleView titleView;

        public override MainSceneId MainSceneId => SampleProductMainSceneId.Title;

        public class TitleTransitionData : ProductTransitionDataBase
        {
            public override MainSceneId MainSceneId => SampleProductMainSceneId.Title;
        }

        [Inject]
        public void Constructor(ISceneManager sceneManager)
        {
            this.sceneManager = sceneManager;
        }

        protected override UniTask OnSetup()
        {
            titleView.Setup(() =>
            {
                sceneManager.TransitionScene(new HomeScene.HomeTransitionData());
            });

            return UniTask.CompletedTask;
        }
    }
}