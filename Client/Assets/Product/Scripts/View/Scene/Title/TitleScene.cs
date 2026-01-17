using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Core.Scene;
using Lighthouse.Core.Scene.SceneBase;
using Product.View.Scene.Common;
using Product.View.Scene.Home;
using UnityEngine;
using VContainer;

namespace Product.View.Scene.Title
{
    public class TitleScene : MainCanvasSceneBase<TitleScene.TitleTransitionData>
    {
        SceneManager sceneManager;

        [SerializeField] TitleView titleView;

        public override MainSceneKey MainSceneId => ProductNameSpace.MainSceneId.Title;

        public class TitleTransitionData : ProductTransitionDataBase
        {
            public override MainSceneKey MainSceneKey => ProductNameSpace.MainSceneId.Title;
        }

        [Inject]
        public void Constructor(SceneManager sceneManager, ICommonSceneManager commonSceneManager)
        {
            this.sceneManager = sceneManager;
        }

        public override UniTask Enter(TransitionDataBase transitionData, TransitionType transitionType, CancellationToken cancelToken)
        {
            titleView.Setup(() =>
            {
                sceneManager.TransitionScene(new HomeScene.HomeTransitionData());
            });

            return base.Enter(transitionData, transitionType, cancelToken);
        }
    }
}