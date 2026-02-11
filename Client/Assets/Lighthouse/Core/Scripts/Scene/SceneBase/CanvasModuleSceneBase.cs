using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Core.Scene.SceneCamera;
using UnityEngine;

namespace Lighthouse.Core.Scene.SceneBase
{
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(SceneCanvasInitializer))]
    public abstract class CanvasModuleSceneBase : ModuleSceneBase, ICanvasSceneBase
    {
        [SerializeField] CanvasGroup canvasGroup;
        [SerializeField] SceneCanvasInitializer canvasInitializer;

        public override UniTask OnLoad()
        {
            canvasGroup.alpha = 0;
            return UniTask.CompletedTask;
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

        protected override void OnBeginInAnimation(TransitionType transitionType, bool withStateChange)
        {
            base.OnBeginInAnimation(transitionType, withStateChange);

            canvasGroup.alpha = withStateChange ? 1.0f : canvasGroup.alpha;
        }

        protected override void OnCompleteOutAnimation(TransitionType transitionType, bool withStateChange)
        {
            base.OnCompleteOutAnimation(transitionType, withStateChange);

            canvasGroup.alpha = withStateChange ? 0.0f : canvasGroup.alpha;
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