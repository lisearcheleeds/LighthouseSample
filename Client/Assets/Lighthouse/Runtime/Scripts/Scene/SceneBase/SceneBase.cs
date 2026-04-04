using System;
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

        public async UniTask Enter(SceneTransitionContext context, CancellationToken cancelToken)
        {
            if (!initialized)
            {
                initialized = true;
                await OnSetup();
            }

            await OnEnter(context, cancelToken);
        }

        public async UniTask Leave(SceneTransitionContext context, CancellationToken cancelToken)
        {
            await OnLeave(context, cancelToken);
        }

        public virtual UniTask SaveSceneState(CancellationToken cancelToken)
        {
            return UniTask.CompletedTask;
        }

        public virtual void ResetInAnimation(SceneTransitionContext context)
        {
        }

        public async UniTask PlayInAnimation(SceneTransitionContext context)
        {
            // NOTE: InAnimation events are independent because they are expected to wait in WhenAll().
            OnBeginInAnimation(context);
            await InAnimation(context);
            OnCompleteInAnimation(context);
        }

        public async UniTask PlayOutAnimation(SceneTransitionContext context)
        {
            // NOTE: OutAnimation events are independent because they are expected to wait in WhenAll().
            OnBeginOutAnimation(context);
            await OutAnimation(context);
            OnCompleteOutAnimation(context);
        }

        public virtual void OnSceneTransitionFinished(SceneTransitionDiff sceneTransitionDiff)
        {
        }

        protected virtual UniTask OnSetup()
        {
            // NOTE: If your setup requires TransitionData then that's not the right idea.
            return UniTask.CompletedTask;
        }

        protected virtual UniTask OnEnter(SceneTransitionContext context, CancellationToken cancelToken)
        {
            return UniTask.CompletedTask;
        }

        protected virtual UniTask OnLeave(SceneTransitionContext context, CancellationToken cancelToken)
        {
            return UniTask.CompletedTask;
        }

        protected virtual void OnBeginInAnimation(SceneTransitionContext context)
        {
        }

        protected virtual UniTask InAnimation(SceneTransitionContext context)
        {
            return UniTask.CompletedTask;
        }

        protected virtual void OnCompleteInAnimation(SceneTransitionContext context)
        {
        }

        protected virtual void OnBeginOutAnimation(SceneTransitionContext context)
        {
        }

        protected virtual UniTask OutAnimation(SceneTransitionContext context)
        {
            return UniTask.CompletedTask;
        }

        protected virtual void OnCompleteOutAnimation(SceneTransitionContext context)
        {
        }
    }
}