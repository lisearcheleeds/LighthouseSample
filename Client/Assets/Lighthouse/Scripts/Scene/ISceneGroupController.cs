using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Scene.SceneTransitionPhase;

namespace Lighthouse.Scene
{
    public interface ISceneGroupController
    {
        public ISceneTransitionPhase CurrentTransitionPhase { get; }

        UniTask<bool> StartTransitionSequence(
            TransitionDataBase transitionData,
            TransitionDirectionType transitionDirectionType,
            TransitionType transitionType,
            CancellationToken cancellationToken);
        UniTask PreReboot();
    }
}