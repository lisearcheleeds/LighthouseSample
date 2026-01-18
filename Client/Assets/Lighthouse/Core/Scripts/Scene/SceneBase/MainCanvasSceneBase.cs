using Lighthouse.Core.Scene.SceneCamera;
using UnityEngine;

namespace Lighthouse.Core.Scene.SceneBase
{
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(SceneCanvasInitializer))]
    public abstract class MainCanvasSceneBase<TTransitionData> : MainSceneBase<TTransitionData>, ICanvasSceneBase where TTransitionData : TransitionDataBase, new()
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

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            canvasInitializer ??= GetComponent<SceneCanvasInitializer>();
            canvasGroup ??= GetComponent<CanvasGroup>();
        }
#endif
    }
}