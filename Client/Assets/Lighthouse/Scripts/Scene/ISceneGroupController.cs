using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Scene.SceneTransitionPhase;

namespace Lighthouse.Scene
{
    public interface ISceneGroupController
    {
        public ISceneTransitionPhase CurrentTransitionPhase { get; }

        UniTask<bool> StartCrossTransitionSequence(TransitionDataBase transitionData, TransitionType transitionType, CancellationToken cancellationToken);
        UniTask<bool> StartExclusiveTransitionSequence(TransitionDataBase transitionData, TransitionType transitionType, CancellationToken cancellationToken);
        UniTask PreReboot();
    }
}