using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace Lighthouse.Scene
{
    public sealed class SceneManager : ISceneManager
    {
        readonly ISceneTransitionController sceneTransitionController;
        readonly IMainSceneManager mainSceneManager;
        readonly IModuleSceneManager moduleSceneManager;
        readonly ISceneGroupProvider sceneGroupProvider;

        public bool IsTransition { get; private set; }

        Stack<TransitionDataBase> transitionDataStack = new();

        SceneGroup currentSceneGroup;

        [Inject]
        public SceneManager(
            ISceneTransitionController sceneTransitionController,
            IMainSceneManager mainSceneManager,
            IModuleSceneManager moduleSceneManager,
            ISceneGroupProvider sceneGroupProvider)
        {
            this.sceneTransitionController = sceneTransitionController;
            this.mainSceneManager = mainSceneManager;
            this.moduleSceneManager = moduleSceneManager;
            this.sceneGroupProvider = sceneGroupProvider;
        }

        async UniTask ISceneManager.TransitionScene(
            TransitionDataBase nextTransitionData,
            TransitionType transitionType,
            MainSceneId backMainSceneId)
        {
            if (IsTransition)
            {
                return;
            }

            var currentSceneTransitionData = transitionDataStack.Count != 0 ? transitionDataStack.Peek() : null;

            await TransitionSceneCore(
                currentSceneTransitionData,
                nextTransitionData,
                TransitionDirectionType.Forward,
                transitionType,
                backMainSceneId,
                CancellationToken.None);
        }

        async UniTask ISceneManager.BackScene(TransitionType transitionType)
        {
            if (IsTransition)
            {
                return;
            }

            if (transitionDataStack.Count < 2)
            {
                return;
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

            if (!backTargetSceneTransitionData.CanTransition || !backTargetSceneTransitionData.CanBackTransition)
            {
                throw new InvalidOperationException(
                    $"Back transition target '{backTargetSceneTransitionData.MainSceneId}' " +
                    "has CanTransition=false or CanBackTransition=false. " +
                    "This indicates an invalid state in the transition stack.");
            }

            await TransitionSceneCore(
                currentSceneTransitionData,
                backTargetSceneTransitionData,
                TransitionDirectionType.Back,
                transitionType,
                null,
                CancellationToken.None);
        }

        async UniTask TransitionSceneCore(
            TransitionDataBase currentTransitionData,
            TransitionDataBase nextTransitionData,
            TransitionDirectionType transitionDirectionType,
            TransitionType transitionType,
            MainSceneId backMainSceneId,
            CancellationToken cancelToken)
        {
            if (!nextTransitionData.CanTransition)
            {
                return;
            }

            var nextSceneGroup = sceneGroupProvider.GetSceneGroup(nextTransitionData.MainSceneId);
            var sceneTransitionDiff = new SceneTransitionDiff(currentSceneGroup, currentTransitionData?.MainSceneId, nextSceneGroup, nextTransitionData.MainSceneId);

            try
            {
                IsTransition = true;

                await sceneTransitionController.StartTransitionSequence(
                    nextTransitionData,
                    sceneTransitionDiff,
                    transitionDirectionType,
                    transitionType,
                    cancelToken);
            }
            catch (Exception e)
            {
                Debug.LogError($"[SceneManager] Exception during scene transition to '{nextTransitionData.MainSceneId}'.\n{e}");
                throw;
            }
            finally
            {
                IsTransition = false;
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
        }

        async UniTask ISceneManager.PreReboot()
        {
            await mainSceneManager.PreReboot();
            await moduleSceneManager.PreReboot();

            transitionDataStack.Clear();
            currentSceneGroup = null;
        }
    }
}