using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
using LighthouseExtends.InputLayer;
using SampleProduct.Input;
using SampleProduct.Input.Layer;
using SampleProduct.LighthouseGenerated;
using SampleProduct.View.Base;
using UnityEngine.InputSystem;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.Purpose
{
    public class PurposeScene : ProductCanvasMainSceneBase<PurposeScene.PurposeTransitionData>
    {
        IPurposePresenter purposePresenter;
        ISceneManager sceneManager;

        public override MainSceneId MainSceneId => SampleProductMainSceneId.Purpose;

        public class PurposeTransitionData : ProductTransitionDataBase
        {
            public override MainSceneId MainSceneId => SampleProductMainSceneId.Purpose;
        }

        [Inject]
        public void Construct(IPurposePresenter purposePresenter, ISceneManager sceneManager)
        {
            this.purposePresenter = purposePresenter;
            this.sceneManager = sceneManager;
        }

        protected override IInputLayer CreateInputLayer(InputActions inputActions) => new DefaultSceneInputLayer(inputActions, () => sceneManager.BackScene());

        protected override InputActionMap GetInputLayerActionMap(InputActions inputActions) => inputActions.Scene;

        protected override UniTask OnSetup()
        {
            purposePresenter.Setup();
            return UniTask.CompletedTask;
        }

        protected override UniTask OnEnter(PurposeTransitionData transitionData, SceneTransitionContext context, CancellationToken cancelToken)
        {
            purposePresenter.OnEnter();
            return UniTask.CompletedTask;
        }
    }
}
