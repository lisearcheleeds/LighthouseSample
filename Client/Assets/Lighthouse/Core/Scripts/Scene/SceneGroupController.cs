using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Core.Scene.SceneCamera;
using Lighthouse.Core.Scene.SceneTransitionPhase;
using UnityEngine;
using VContainer;

namespace Lighthouse.Core.Scene
{
    public class SceneGroupController : ISceneGroupController
    {
        readonly IMainSceneGroupProvider mainSceneGroupProvider;
        readonly ICommonSceneManager commonSceneManager;
        readonly ISceneCameraManager sceneCameraManager;
        readonly IInputBlocker inputBlocker;

        public MainSceneKey CurrentMainSceneKey => currentMainSceneGroup?.CurrentScene != null ? currentMainSceneGroup.CurrentScene.MainSceneId : null;
        public ISceneTransitionPhase CurrentTransitionPhase { get; private set; }

        protected virtual ISceneTransitionPhase[] CrossTransitionPhaseSet { get; } =
        {
            new StartTransitionPhase(),
            new LoadScenePhase(),
            new EnterScenePhase(),
            new CrossAnimationPhase(),
            new LeaveScenePhase(),
            new UnloadScenePhase(),
            new FinishTransitionPhase(),
        };

        protected virtual ISceneTransitionPhase[] ExclusiveTransitionPhaseSet { get; } =
        {
            new StartTransitionPhase(),
            new OutAnimationPhase(),
            new LeaveScenePhase(),
            new LoadScenePhase(),
            new UnloadScenePhase(),
            new EnterScenePhase(),
            new InAnimationPhase(),
            new FinishTransitionPhase(),
        };

        MainSceneGroup currentMainSceneGroup;

        [Inject]
        public SceneGroupController(
            IMainSceneGroupProvider mainSceneGroupProvider,
            ICommonSceneManager commonSceneManager,
            ISceneCameraManager sceneCameraManager,
            IInputBlocker inputBlocker)
        {
            this.mainSceneGroupProvider = mainSceneGroupProvider;
            this.commonSceneManager = commonSceneManager;
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
            await currentMainSceneGroup.Leave(null, TransitionType.Default, CancellationToken.None);
        }

        protected virtual async UniTask<bool> StartTransition(
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

            var beforeMainSceneKey = CurrentMainSceneKey;
            var afterMainSceneGroup = mainSceneGroupProvider.GetMainSceneGroup(transitionData.MainSceneKey);

            inputBlocker.Block<SceneGroupController>();

            foreach (var transitionPhase in transitionPhases)
            {
                CurrentTransitionPhase = transitionPhase;

                Debug.Log($"SceneTransition PhaseChanged: {CurrentTransitionPhase.GetType()}");

                var tasks = transitionPhase.Steps.Select(step => step.Run(
                    transitionData,
                    transitionType,
                    beforeMainSceneKey,
                    currentMainSceneGroup,
                    afterMainSceneGroup,
                    sceneCameraManager,
                    commonSceneManager,
                    cancellationToken
                ));

                await UniTask.WhenAll(tasks);
            }

            inputBlocker.UnBlock<SceneGroupController>();

            currentMainSceneGroup = afterMainSceneGroup;

            Application.backgroundLoadingPriority = prevPriority;

            // sw.Stop();
            // MessageBroker.NotifySceneTransitionFinished(prevMainSceneGroup.CurrentMainSceneKey, transitionData.CurrentMainSceneKey, (int)sw.ElapsedMilliseconds)

            CurrentTransitionPhase = null;
            return true;
        }
    }
}