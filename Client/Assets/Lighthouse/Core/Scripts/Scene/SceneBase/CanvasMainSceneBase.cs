using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Core.Scene.SceneCamera;
using UnityEngine;

namespace Lighthouse.Core.Scene.SceneBase
{
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(SceneCanvasInitializer))]
    public abstract class CanvasMainSceneBase<TTransitionData> : MainSceneBase<TTransitionData>, ICanvasSceneBase where TTransitionData : TransitionDataBase, new()
    {
        [SerializeField] CanvasGroup canvasGroup;
        [SerializeField] SceneCanvasInitializer canvasInitializer;

        public override UniTask OnLoad()
        {
            canvasGroup.alpha = 0;
            return base.OnLoad();
        }

        protected override UniTask OnEnter(TransitionDataBase transitionData, TransitionType transitionType, CancellationToken cancelToken)
        {
            canvasGroup.alpha = 1;
            return base.OnEnter(transitionData, transitionType, cancelToken);
        }

        public virtual void InitializeCanvas(ISceneCamera canvasCamera)
        {
            canvasInitializer.Initialize(canvasCamera);
        }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            canvasInitializer ??= GetComponent<SceneCanvasInitializer>();
            canvasGroup ??= GetComponent<CanvasGroup>();
        }
#endif
    }
}