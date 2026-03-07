using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Input;
using Lighthouse.Scene.SceneCamera;
using Lighthouse.Scene.SceneTransitionPhase;
using UnityEngine;
using VContainer;

namespace Lighthouse.Scene
{
    public sealed class SceneGroupController : ISceneGroupController
    {
        readonly ISceneGroupProvider sceneGroupProvider;
        readonly IMainSceneManager mainSceneManager;
        readonly IModuleSceneManager moduleSceneManager;
        readonly ISceneCameraManager sceneCameraManager;
        readonly ISceneTransitionSequenceProvider sceneTransitionSequenceProvider;
        readonly IInputBlocker inputBlocker;

        public ISceneTransitionPhase CurrentTransitionPhase { get; private set; }

        SceneGroup currentSceneGroup;

        [Inject]
        public SceneGroupController(
            ISceneGroupProvider sceneGroupProvider,
            IMainSceneManager mainSceneManager,
            IModuleSceneManager moduleSceneManager,
            ISceneCameraManager sceneCameraManager,
            ISceneTransitionSequenceProvider sceneTransitionSequenceProvider,
            IInputBlocker inputBlocker)
        {
            this.sceneGroupProvider = sceneGroupProvider;
            this.mainSceneManager = mainSceneManager;
            this.moduleSceneManager = moduleSceneManager;
            this.sceneCameraManager = sceneCameraManager;
            this.sceneTransitionSequenceProvider = sceneTransitionSequenceProvider;
            this.inputBlocker = inputBlocker;
        }

        async UniTask<bool> ISceneGroupController.StartCrossTransitionSequence(
            TransitionDataBase transitionData,
            TransitionType transitionType,
            CancellationToken cancellationToken)
        {
            return await StartTransition(
                sceneTransitionSequenceProvider.CrossSequence,
                transitionData,
                transitionType,
                cancellationToken);
        }

        async UniTask<bool> ISceneGroupController.StartExclusiveTransitionSequence(
            TransitionDataBase transitionData,
            TransitionType transitionType,
            CancellationToken cancellationToken)
        {
            return await StartTransition(
                sceneTransitionSequenceProvider.ExclusiveSequence,
                transitionData,
                transitionType,
                cancellationToken);
        }

        async UniTask ISceneGroupController.PreReboot()
        {
            await mainSceneManager.Leave(null, TransitionType.Default, null, CancellationToken.None);
        }

        async UniTask<bool> StartTransition(
            ISceneTransitionPhase[] transitionPhases,
            TransitionDataBase transitionData,
            TransitionType transitionType,
            CancellationToken cancellationToken)
        {
            if (!CurrentTransitionPhase?.CanTransitionIntercept ?? false)
            {
                Debug.LogError($"[SceneTransitionWorker] Scene transition is not possible in the current phase. {CurrentTransitionPhase}");
                return false;
            }

            CurrentTransitionPhase = null;

            // MessageBroker.NotifySceneTransitionStart()
            // var sw = new System.Diagnostics.Stopwatch();
            // sw.Start();

            var prevPriority = Application.backgroundLoadingPriority;
            Application.backgroundLoadingPriority = UnityEngine.ThreadPriority.High;

            var afterSceneGroup = sceneGroupProvider.GetSceneGroup(transitionData.MainSceneId);
            var sceneTransitionDiff = new SceneTransitionDiff(currentSceneGroup, mainSceneManager.CurrentMainSceneId, afterSceneGroup, transitionData.MainSceneId);
            var context = new SceneTransitionContext(transitionData, transitionType, sceneTransitionDiff, mainSceneManager, moduleSceneManager, sceneCameraManager);

            inputBlocker.Block<SceneGroupController>();

            try
            {
                foreach (var transitionPhase in transitionPhases)
                {
                    CurrentTransitionPhase = transitionPhase;

                    var tasks = transitionPhase.Steps.Select(step => step.Run(context, cancellationToken));

                    await UniTask.WhenAll(tasks);
                }
            }
            finally
            {
                inputBlocker.UnBlock<SceneGroupController>();
            }

            currentSceneGroup = afterSceneGroup;

            Application.backgroundLoadingPriority = prevPriority;

            // sw.Stop();
            // MessageBroker.NotifySceneTransitionFinished(prevMainSceneGroup.CurrentMainSceneId, transitionData.CurrentMainSceneId, (int)sw.ElapsedMilliseconds)

            CurrentTransitionPhase = null;
            return true;
        }
    }
}