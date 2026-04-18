using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
using Lighthouse.Scene.SceneBase;
using LighthouseExtends.Animation.Runtime;
using LighthouseExtends.InputLayer;
using SampleProduct.Input;
using SampleProduct.Input.Layer;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace SampleProduct.View.Base
{
    [RequireComponent(typeof(LHSceneTransitionAnimatorManager))]
    public abstract class ProductCanvasMainSceneBase<TTransitionData> : CanvasMainSceneBase<TTransitionData> where TTransitionData : ProductTransitionDataBase
    {
        [SerializeField] LHSceneTransitionAnimatorManager sceneTransitionAnimatorManager;

        ISceneManager sceneManager;
        IInputLayerController inputLayerController;
        IInputLayer currentInputLayer;

        InputActions inputActions;

        [Inject]
        public void ConstructInputLayer(
            ISceneManager sceneManager,
            IInputLayerController inputLayerController,
            InputActions inputActions)
        {
            this.sceneManager = sceneManager;
            this.inputLayerController = inputLayerController;
            this.inputActions = inputActions;
        }

        protected virtual IInputLayer CreateInputLayer(InputActions inputActions)
        {
            return null;
        }

        protected virtual InputActionMap GetInputLayerActionMap(InputActions inputActions)
        {
            return inputActions.Scene;
        }

        protected override async UniTask OnEnter(SceneTransitionContext context, CancellationToken cancelToken)
        {
            var layer = CreateInputLayer(inputActions);
            var actionMap = GetInputLayerActionMap(inputActions);
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
