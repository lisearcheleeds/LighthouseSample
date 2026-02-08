using Cysharp.Threading.Tasks;
using Lighthouse.Core.Scene;
using ProductNameSpace;
using SampleProduct.View.Scene.MainScene.Home;
using SampleProduct.View.Scene.SceneBase;
using UnityEngine;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.Title
{
    public class TitleScene : ProductMainCanvasSceneBase<TitleScene.TitleTransitionData>
    {
        SceneManager sceneManager;

        [SerializeField] TitleView titleView;

        public override MainSceneId MainSceneId => SampleProductMainSceneId.Title;

        public class TitleTransitionData : ProductTransitionDataBase
        {
            public override MainSceneId MainSceneId => SampleProductMainSceneId.Title;
        }

        [Inject]
        public void Constructor(SceneManager sceneManager, ISceneModuleManager sceneModuleManager)
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