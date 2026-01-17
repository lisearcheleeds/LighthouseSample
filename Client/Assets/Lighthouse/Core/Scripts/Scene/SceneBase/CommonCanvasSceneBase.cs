using UnityEngine;

namespace Lighthouse.Core.Scene.SceneBase
{
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(SceneCanvasInitializer))]
    public abstract class CommonCanvasSceneBase : CommonSceneBase, ICanvasSceneBase
    {
        [SerializeField] CanvasGroup canvasGroup;
        [SerializeField] SceneCanvasInitializer canvasInitializer;
        ISceneCamera[] placeholderCameras;

        public override ISceneCamera[] GetSceneCameraList()
        {
            return placeholderCameras;
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
        protected override void OnValidate()
        {
            base.OnValidate();

            canvasInitializer ??= GetComponent<SceneCanvasInitializer>();
            canvasGroup ??= GetComponent<CanvasGroup>();
        }
#endif
    }
}