using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer;

namespace Lighthouse.Scene
{
    public sealed class SceneManager : ISceneManager
    {
        readonly ISceneTransitionController sceneTransitionController;
        readonly IMainSceneManager mainSceneManager;
        readonly ISceneGroupProvider sceneGroupProvider;

        public bool IsTransition { get; private set; }

        Stack<TransitionDataBase> transitionDataStack = new();

        SceneGroup currentSceneGroup;

        [Inject]
        public SceneManager(
            ISceneTransitionController sceneTransitionController,
            IMainSceneManager mainSceneManager,
            ISceneGroupProvider sceneGroupProvider)
        {
            this.sceneTransitionController = sceneTransitionController;
            this.mainSceneManager = mainSceneManager;
            this.sceneGroupProvider = sceneGroupProvider;
        }

        void ISceneManager.TransitionScene(TransitionDataBase nextTransitionData, TransitionType transitionType, MainSceneId backMainSceneId, Action<bool> onComplete)
        {
            if (IsTransition)
            {
                return;
            }

            var currentSceneTransitionData = transitionDataStack.Count != 0 ? transitionDataStack.Peek() : null;

            UniTask.Void(async () =>
            {
                var isSuccess = await TransitionSceneAsync(
                    currentSceneTransitionData,
                    nextTransitionData,
                    TransitionDirectionType.Forward,
                    transitionType,
                    backMainSceneId);
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

            return await TransitionSceneAsync(
                currentSceneTransitionData,
                backTargetSceneTransitionData,
                TransitionDirectionType.Back,
                transitionType,
                null);
        }

        async UniTask<bool> TransitionSceneAsync(
            TransitionDataBase currentTransitionData,
            TransitionDataBase nextTransitionData,
            TransitionDirectionType transitionDirectionType,
            TransitionType transitionType,
            MainSceneId backMainSceneId)
        {
            if (!nextTransitionData.CanTransition)
            {
                return false;
            }

            var nextSceneGroup = sceneGroupProvider.GetSceneGroup(nextTransitionData.MainSceneId);

            IsTransition = true;

            var sceneTransitionDiff = new SceneTransitionDiff(currentSceneGroup, currentTransitionData?.MainSceneId, nextSceneGroup, nextTransitionData.MainSceneId);

            var transitionSuccess = await sceneTransitionController.StartTransitionSequence(
                nextTransitionData,
                sceneTransitionDiff,
                transitionDirectionType,
                transitionType,
                CancellationToken.None);

            IsTransition = false;

            if (!transitionSuccess)
            {
                return false;
            }

            currentSceneGroup = nextSceneGroup;

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

        async UniTask ISceneManager.PreReboot()
        {
            await mainSceneManager.Leave(null, CancellationToken.None);
        }
    }
}