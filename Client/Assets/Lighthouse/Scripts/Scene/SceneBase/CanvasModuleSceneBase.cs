using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Scene.SceneCamera;
using UnityEngine;

namespace Lighthouse.Scene.SceneBase
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
            return base.OnLoad();
        }

        protected override UniTask OnEnter(TransitionDataBase transitionData, TransitionType transitionType, SceneTransitionDiff sceneTransitionDiff, CancellationToken cancelToken)
        {
            if (sceneTransitionDiff.ActivateSceneModuleIds.Contains(ModuleSceneId))
            {
                canvasGroup.alpha = 1;
            }

            return base.OnEnter(transitionData, transitionType, sceneTransitionDiff, cancelToken);
        }

        protected override UniTask OnLeave(TransitionDataBase transitionData, TransitionType transitionType, SceneTransitionDiff sceneTransitionDiff, CancellationToken cancelToken)
        {
            if (sceneTransitionDiff.DeactivateSceneModuleIds.Contains(ModuleSceneId))
            {
                canvasGroup.alpha = 0;
            }

            return base.OnLeave(transitionData, transitionType, sceneTransitionDiff, cancelToken);
        }

        public virtual void InitializeCanvas(ISceneCamera canvasCamera)
        {
            canvasInitializer.Initialize(canvasCamera);
        }

        protected override void OnBeginInAnimation(TransitionType transitionType, bool isActivateScene)
        {
            base.OnBeginInAnimation(transitionType, isActivateScene);

            canvasGroup.alpha = isActivateScene ? 1.0f : canvasGroup.alpha;
        }

        protected override void OnCompleteOutAnimation(TransitionType transitionType, bool isDeactivateScene)
        {
            base.OnCompleteOutAnimation(transitionType, isDeactivateScene);

            canvasGroup.alpha = isDeactivateScene ? 0.0f : canvasGroup.alpha;
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