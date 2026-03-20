using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Scene.SceneTransitionPhase;
using VContainer;

namespace Lighthouse.Scene
{
    public sealed class SceneManager : ISceneManager
    {
        readonly ISceneGroupController sceneGroupController;

        public ISceneTransitionPhase CurrentTransitionPhase => sceneGroupController.CurrentTransitionPhase;
        public bool IsTransition => CurrentTransitionPhase != null;

        Stack<TransitionDataBase> transitionDataStack = new();

        [Inject]
        public SceneManager(ISceneGroupController sceneGroupController)
        {
            this.sceneGroupController = sceneGroupController;
        }

        void ISceneManager.TransitionScene(TransitionDataBase nextTransitionData, TransitionType transitionType, MainSceneId backMainSceneId, Action<bool> onComplete)
        {
            if (IsTransition)
            {
                return;
            }

            UniTask.Void(async () =>
            {
                var isSuccess = await TransitionSceneAsync(nextTransitionData, TransitionDirectionType.Forward, transitionType, backMainSceneId);
                onComplete?.Invoke(isSuccess);
            });
        }

        void ISceneManager.BackScene(TransitionType transitionType)
        {
            if (IsTransition)
            {
                return;
            }

            if (!transitionDataStack.Any())
            {
                return;
            }

            BackSceneAsync(transitionType).Forget();
        }

        async UniTask<bool> BackSceneAsync(TransitionType transitionType)
        {
            if (transitionDataStack.Count < 2)
            {
                return false;
            }

            var currentSceneTransitionData = transitionDataStack.Pop();
            var backTargetSceneTransitionData = transitionDataStack.Pop();

            while (true)
            {
                if (!transitionDataStack.Any())
                {
                    break;
                }

                if (backTargetSceneTransitionData.CanTransition
                    && backTargetSceneTransitionData.CanBackTransition
                    && currentSceneTransitionData.MainSceneId != backTargetSceneTransitionData.MainSceneId)
                {
                    break;
                }

                backTargetSceneTransitionData = transitionDataStack.Pop();
            }

            return await TransitionSceneAsync(backTargetSceneTransitionData, TransitionDirectionType.Back, transitionType, null);
        }

        async UniTask<bool> TransitionSceneAsync(
            TransitionDataBase nextTransitionData,
            TransitionDirectionType transitionDirectionType,
            TransitionType transitionType,
            MainSceneId backMainSceneId)
        {
            if (!nextTransitionData.CanTransition)
            {
                return false;
            }

            var transitionSuccess = await sceneGroupController.StartTransitionSequence(
                nextTransitionData,
                transitionDirectionType,
                transitionType,
                CancellationToken.None);
            if (!transitionSuccess)
            {
                return false;
            }

            transitionDataStack.Push(nextTransitionData);

            if (backMainSceneId != null)
            {
                while (transitionDataStack.Count > 0 && transitionDataStack.Peek().MainSceneId != backMainSceneId)
                {
                    transitionDataStack.Pop();
                }
            }

            // await new WaitWhile(() => PopupManager.Instance.IsOpen(CurrentMainSceneId) || LoadingManager.Instance.IsShowGuard);

            return true;
        }
    }
}