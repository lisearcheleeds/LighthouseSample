using System;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Lighthouse.Core.Scene
{
    [RequireComponent(typeof(SceneTransitionAnimatorManager))]
    public abstract class SceneBase : MonoBehaviour
    {
        [SerializeField] SceneTransitionAnimatorManager sceneTransitionAnimatorManager;

        public virtual ISceneCamera[] GetSceneCameraList()
        {
            return null;
        }

        public virtual UniTask OnLoad()
        {
            return UniTask.CompletedTask;
        }

        public virtual UniTask OnUnload()
        {
            return UniTask.CompletedTask;
        }

        public virtual UniTask Enter(TransitionDataBase transitionData, TransitionType transitionType, CancellationToken cancelToken)
        {
            return UniTask.CompletedTask;
        }

        public virtual UniTask Leave(TransitionDataBase transitionData, TransitionType transitionType, CancellationToken cancelToken)
        {
            return UniTask.CompletedTask;
        }

        public virtual UniTask SaveSceneState(CancellationToken cancelToken)
        {
            return UniTask.CompletedTask;
        }

        public virtual void OnSuspend()
        {
        }

        public virtual void OnResume()
        {
        }

        public async UniTask ResetAnimation(TransitionType transitionType)
        {
            await sceneTransitionAnimatorManager.ResetAnimation(transitionType);
        }

        public async UniTask InAnimation(TransitionType transitionType)
        {
            await OnBeginInAnimation(transitionType);
            await sceneTransitionAnimatorManager.In(transitionType);
            await OnCompleteInAnimation(transitionType);
        }

        public async UniTask OutAnimation(TransitionType transitionType)
        {
            await OnBeginOutAnimation(transitionType);
            await sceneTransitionAnimatorManager.Out(transitionType);
            await OnCompleteOutAnimation(transitionType);
        }

        public virtual void OnSceneTransitionFinished()
        {
        }

        protected virtual UniTask OnBeginInAnimation(TransitionType transitionType)
        {
            gameObject.SetActive(true);
            return UniTask.CompletedTask;
        }

        protected virtual UniTask OnCompleteInAnimation(TransitionType transitionType)
        {
            return UniTask.CompletedTask;
        }

        protected virtual UniTask OnBeginOutAnimation(TransitionType transitionType)
        {
            return UniTask.CompletedTask;
        }

        protected virtual UniTask OnCompleteOutAnimation(TransitionType transitionType)
        {
            gameObject.SetActive(false);
            return UniTask.CompletedTask;
        }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            sceneTransitionAnimatorManager ??= GetComponent<SceneTransitionAnimatorManager>();
        }
#endif
    }
}