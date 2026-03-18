using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Scene.SceneCamera;
using UnityEngine;

namespace Lighthouse.Scene.SceneBase
{
    public abstract class SceneBase : MonoBehaviour
    {
        bool initialized;

        public virtual ISceneCamera[] GetSceneCameraList()
        {
            return Array.Empty<ISceneCamera>();
        }

        public virtual UniTask OnLoad()
        {
            gameObject.SetActive(false);
            return UniTask.CompletedTask;
        }

        public virtual UniTask OnUnload()
        {
            return UniTask.CompletedTask;
        }

        public async UniTask Enter(TransitionDataBase transitionData, TransitionType transitionType, SceneTransitionDiff sceneTransitionDiff, CancellationToken cancelToken)
        {
            if (!initialized)
            {
                initialized = true;
                await OnSetup();
            }

            await OnEnter(transitionData, transitionType, sceneTransitionDiff, cancelToken);
        }

        public async UniTask Leave(TransitionDataBase transitionData, TransitionType transitionType, SceneTransitionDiff sceneTransitionDiff, CancellationToken cancelToken)
        {
            await OnLeave(transitionData, transitionType, sceneTransitionDiff, cancelToken);
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

        public virtual void ResetInAnimation(TransitionType transitionType)
        {
        }

        public async UniTask PlayInAnimation(TransitionType transitionType, bool isActivateScene)
        {
            OnBeginInAnimation(transitionType, isActivateScene);
            await InAnimation(transitionType, isActivateScene);
            OnCompleteInAnimation(transitionType, isActivateScene);
        }

        public async UniTask PlayOutAnimation(TransitionType transitionType, bool isDeactivateScene)
        {
            OnBeginOutAnimation(transitionType, isDeactivateScene);
            await OutAnimation(transitionType, isDeactivateScene);
            OnCompleteOutAnimation(transitionType, isDeactivateScene);
        }

        public virtual void OnSceneTransitionFinished(SceneTransitionDiff sceneTransitionDiff)
        {
        }

        protected virtual UniTask OnSetup()
        {
            // NOTE: If your setup requires TransitionData then that's not the right idea.
            return UniTask.CompletedTask;
        }

        protected virtual UniTask OnEnter(TransitionDataBase transitionData, TransitionType transitionType, SceneTransitionDiff sceneTransitionDiff, CancellationToken cancelToken)
        {
            return UniTask.CompletedTask;
        }

        protected virtual UniTask OnLeave(TransitionDataBase transitionData, TransitionType transitionType, SceneTransitionDiff sceneTransitionDiff, CancellationToken cancelToken)
        {
            return UniTask.CompletedTask;
        }

        protected virtual void OnBeginInAnimation(TransitionType transitionType, bool isActivateScene)
        {
        }

        protected virtual UniTask InAnimation(TransitionType transitionType, bool isActivateScene)
        {
            return UniTask.CompletedTask;
        }

        protected virtual void OnCompleteInAnimation(TransitionType transitionType, bool isActivateScene)
        {
        }

        protected virtual void OnBeginOutAnimation(TransitionType transitionType, bool isDeactivateScene)
        {
        }

        protected virtual UniTask OutAnimation(TransitionType transitionType, bool isDeactivateScene)
        {
            return UniTask.CompletedTask;
        }

        protected virtual void OnCompleteOutAnimation(TransitionType transitionType, bool isDeactivateScene)
        {
        }
    }
}