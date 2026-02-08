using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Core.Scene.SceneCamera;
using UnityEngine;

namespace Lighthouse.Core.Scene.SceneBase
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
            return UniTask.CompletedTask;
        }

        public virtual UniTask OnUnload()
        {
            return UniTask.CompletedTask;
        }

        public async UniTask Enter(TransitionDataBase transitionData, TransitionType transitionType, CancellationToken cancelToken)
        {
            if (!initialized)
            {
                initialized = true;
                await OnSetup();
            }

            await OnEnter(transitionData, transitionType, cancelToken);
        }

        public async UniTask Leave(TransitionDataBase transitionData, TransitionType transitionType, CancellationToken cancelToken)
        {
            await OnLeave(transitionData, transitionType, cancelToken);
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

        public async UniTask PlayInAnimation(TransitionType transitionType, bool withStateChange)
        {
            OnBeginInAnimation(transitionType, withStateChange);
            await InAnimation(transitionType, withStateChange);
            OnCompleteInAnimation(transitionType, withStateChange);
        }

        public async UniTask PlayOutAnimation(TransitionType transitionType, bool withStateChange)
        {
            OnBeginOutAnimation(transitionType, withStateChange);
            await OutAnimation(transitionType, withStateChange);
            OnCompleteOutAnimation(transitionType, withStateChange);
        }

        public virtual void OnSceneTransitionFinished(SceneTransitionDiff sceneTransitionDiff)
        {
        }

        protected virtual UniTask OnSetup()
        {
            // NOTE: If your setup requires TransitionData then that's not the right idea.
            return UniTask.CompletedTask;
        }

        protected virtual UniTask OnEnter(TransitionDataBase transitionData, TransitionType transitionType, CancellationToken cancelToken)
        {
            return UniTask.CompletedTask;
        }

        protected virtual UniTask OnLeave(TransitionDataBase transitionData, TransitionType transitionType, CancellationToken cancelToken)
        {
            return UniTask.CompletedTask;
        }

        protected virtual void OnBeginInAnimation(TransitionType transitionType, bool withStateChange)
        {
        }

        protected virtual UniTask InAnimation(TransitionType transitionType, bool withStateChange)
        {
            return UniTask.CompletedTask;
        }

        protected virtual void OnCompleteInAnimation(TransitionType transitionType, bool withStateChange)
        {
        }

        protected virtual void OnBeginOutAnimation(TransitionType transitionType, bool withStateChange)
        {
        }

        protected virtual UniTask OutAnimation(TransitionType transitionType, bool withStateChange)
        {
            return UniTask.CompletedTask;
        }

        protected virtual void OnCompleteOutAnimation(TransitionType transitionType, bool withStateChange)
        {
        }
    }
}