using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Lighthouse.Core.Scene.SceneBase
{
    public abstract class SceneBase : MonoBehaviour
    {
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

        public async UniTask PlayResetAnimation(TransitionType transitionType)
        {
            await ResetAnimation(transitionType);
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

        public virtual void OnSceneTransitionFinished()
        {
        }

        protected virtual UniTask ResetAnimation(TransitionType transitionType)
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