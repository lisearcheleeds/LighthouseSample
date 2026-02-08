using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Core.Input;
using Lighthouse.Core.Scene.SceneCamera;
using Lighthouse.Core.Scene.SceneTransitionPhase;
using UnityEngine;
using VContainer;

namespace Lighthouse.Core.Scene
{
    public sealed class SceneGroupController : ISceneGroupController
    {
        readonly ISceneGroupProvider sceneGroupProvider;
        readonly IMainSceneManager mainSceneManager;
        readonly ISceneModuleManager sceneModuleManager;
        readonly ISceneCameraManager sceneCameraManager;
        readonly IInputBlocker inputBlocker;

        public ISceneTransitionPhase CurrentTransitionPhase { get; private set; }

        ISceneTransitionPhase[] CrossTransitionPhaseSet { get; } =
        {
            new StartTransitionPhase(),
            new LoadSceneGroupPhase(),
            new EnterScenePhase(),
            new CrossAnimationPhase(),
            new LeaveScenePhase(),
            new UnloadSceneGroupPhase(),
            new FinishTransitionPhase(),
        };

        ISceneTransitionPhase[] ExclusiveTransitionPhaseSet { get; } =
        {
            new StartTransitionPhase(),
            new OutAnimationPhase(),
            new LeaveScenePhase(),
            new LoadSceneGroupPhase(),
            new UnloadSceneGroupPhase(),
            new EnterScenePhase(),
            new InAnimationPhase(),
            new FinishTransitionPhase(),
        };

        SceneGroup currentSceneGroup;

        [Inject]
        public SceneGroupController(
            ISceneGroupProvider sceneGroupProvider,
            IMainSceneManager mainSceneManager,
            ISceneModuleManager sceneModuleManager,
            ISceneCameraManager sceneCameraManager,
            IInputBlocker inputBlocker)
        {
            this.sceneGroupProvider = sceneGroupProvider;
            this.mainSceneManager = mainSceneManager;
            this.sceneModuleManager = sceneModuleManager;
            this.sceneCameraManager = sceneCameraManager;
            this.inputBlocker = inputBlocker;
        }

        async UniTask<bool> ISceneGroupController.StartCrossTransitionSequence(
            TransitionDataBase transitionData,
            TransitionType transitionType,
            CancellationToken cancellationToken)
        {
            return await StartTransition(
                CrossTransitionPhaseSet,
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
                ExclusiveTransitionPhaseSet,
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

            inputBlocker.Block<SceneGroupController>();

            foreach (var transitionPhase in transitionPhases)
            {
                CurrentTransitionPhase = transitionPhase;

                Debug.Log($"SceneTransition PhaseChanged: {CurrentTransitionPhase.GetType()}");

                var tasks = transitionPhase.Steps.Select(step => step.Run(
                    transitionData,
                    transitionType,
                    sceneTransitionDiff,
                    mainSceneManager,
                    sceneModuleManager,
                    sceneCameraManager,
                    cancellationToken
                ));

                await UniTask.WhenAll(tasks);
            }

            inputBlocker.UnBlock<SceneGroupController>();

            currentSceneGroup = afterSceneGroup;

            Application.backgroundLoadingPriority = prevPriority;

            // sw.Stop();
            // MessageBroker.NotifySceneTransitionFinished(prevMainSceneGroup.CurrentMainSceneId, transitionData.CurrentMainSceneId, (int)sw.ElapsedMilliseconds)

            CurrentTransitionPhase = null;
            return true;
        }
    }
}