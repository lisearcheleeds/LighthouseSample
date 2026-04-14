using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
using Lighthouse.Scene.SceneBase;
using LighthouseExtends.Animation.Runtime;
using LighthouseExtends.InputLayer;
using UnityEngine;
using VContainer;

namespace SampleProduct.View.Base
{
    [RequireComponent(typeof(LHSceneTransitionAnimatorManager))]
    public abstract class ProductCanvasMainSceneBase<TTransitionData> : CanvasMainSceneBase<TTransitionData> where TTransitionData : ProductTransitionDataBase
    {
        [SerializeField] LHSceneTransitionAnimatorManager sceneTransitionAnimatorManager;

        IInputLayerController inputLayerController;
        InputLayer currentInputLayer;

        [Inject]
        public void ConstructInputLayer(IInputLayerController inputLayerController)
        {
            this.inputLayerController = inputLayerController;
        }

        /// <summary>
        /// このSceneで使用するInputLayerを返す。
        /// null を返した場合はInputLayerの操作を行わない。
        /// </summary>
        protected virtual InputLayer CreateInputLayer() => null;

        protected override async UniTask OnEnter(SceneTransitionContext context, CancellationToken cancelToken)
        {
            currentInputLayer = CreateInputLayer();
            if (currentInputLayer != null)
            {
                inputLayerController.PushLayer(currentInputLayer);
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
