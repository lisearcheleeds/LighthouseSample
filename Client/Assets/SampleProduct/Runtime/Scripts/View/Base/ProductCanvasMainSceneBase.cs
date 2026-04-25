using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
using Lighthouse.Scene.SceneBase;
using LighthouseExtends.Addressable;
using LighthouseExtends.Animation;
using LighthouseExtends.InputLayer;
using SampleProduct.Core;
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

        ISampleSceneManager sceneManager;
        IInputLayerController inputLayerController;
        IInputLayer currentInputLayer;
        ILHAssetManager assetManager;

        InputActions inputActions;

        protected ILHAssetScope AssetScope { get; private set; }

        [Inject]
        public void Construct(
            ISampleSceneManager sceneManager,
            IInputLayerController inputLayerController,
            InputActions inputActions,
            ILHAssetManager assetManager)
        {
            this.sceneManager = sceneManager;
            this.inputLayerController = inputLayerController;
            this.inputActions = inputActions;
            this.assetManager = assetManager;
        }

        protected override UniTask OnSetup()
        {
            AssetScope = assetManager.CreateScope();
            return base.OnSetup();
        }

        public override async UniTask OnUnload()
        {
            await base.OnUnload();
            AssetScope?.Dispose();
            AssetScope = null;
        }

        protected virtual IInputLayer CreateInputLayer(InputActions inputActions)
        {
            return null;
        }

        protected virtual InputActionMap GetInputLayerActionMap(InputActions inputActions)
        {
            return inputActions.Scene;
        }

        protected override async UniTask OnEnter(ISceneTransitionContext context, CancellationToken cancelToken)
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

        protected override async UniTask OnLeave(ISceneTransitionContext context, CancellationToken cancelToken)
        {
            await base.OnLeave(context, cancelToken);
            if (currentInputLayer != null)
            {
                inputLayerController.PopLayer(currentInputLayer);
                currentInputLayer = null;
            }
        }

        public override void ResetInAnimation(ISceneTransitionContext context)
        {
            sceneTransitionAnimatorManager.ResetInAnimation();
        }

        protected override async UniTask InAnimation(ISceneTransitionContext context)
        {
            await sceneTransitionAnimatorManager.InAnimation();
        }

        protected override async UniTask OutAnimation(ISceneTransitionContext context)
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
