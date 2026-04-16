using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
using Lighthouse.Scene.SceneBase;
using LighthouseExtends.Animation.Runtime;
using LighthouseExtends.InputLayer;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace SampleProduct.View.Base
{
    [RequireComponent(typeof(LHSceneTransitionAnimatorManager))]
    public abstract class ProductCanvasMainSceneBase<TTransitionData> : CanvasMainSceneBase<TTransitionData> where TTransitionData : ProductTransitionDataBase
    {
        [SerializeField] LHSceneTransitionAnimatorManager sceneTransitionAnimatorManager;

        IInputLayerController inputLayerController;
        IInputLayer currentInputLayer;

        protected PlayerInputActions playerInputActions;

        [Inject]
        public void ConstructInputLayer(IInputLayerController inputLayerController, PlayerInputActions playerInputActions)
        {
            this.inputLayerController = inputLayerController;
            this.playerInputActions = playerInputActions;
        }

        protected virtual IInputLayer CreateInputLayer() => null;

        protected virtual InputActionMap GetInputLayerActionMap() => null;

        protected override async UniTask OnEnter(SceneTransitionContext context, CancellationToken cancelToken)
        {
            var layer = CreateInputLayer();
            var actionMap = GetInputLayerActionMap();
            if (layer != null && actionMap != null)
            {
                currentInputLayer = layer;
                inputLayerController.PushLayer(currentInputLayer, actionMap);
            }
            await base.OnEnter(context, cancelToken);
        }

        protected override async UniTask OnLeave(SceneTransitionContext context, CancellationToken cancelToken)
        {
            await base.OnLeave(context, cancelToken);
            if (currentInputLayer != null)
            {
                inputLayerController.PopLayer(currentInputLayer);
                currentInputLayer = null;
            }
        }

        public override void ResetInAnimation(SceneTransitionContext context)
        {
            sceneTransitionAnimatorManager.ResetInAnimation();
        }

        protected override async UniTask InAnimation(SceneTransitionContext context)
        {
            await sceneTransitionAnimatorManager.InAnimation();
        }

        protected override async UniTask OutAnimation(SceneTransitionContext context)
        {
            await sceneTransitionAnimatorManager.OutAnimation();
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            sceneTransitionAnimatorManager ??= GetComponent<LHSceneTransitionAnimatorManager>();
        }
#endif
    }
}
