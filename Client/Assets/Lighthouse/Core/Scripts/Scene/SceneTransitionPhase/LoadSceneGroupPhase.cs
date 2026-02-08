using Lighthouse.Core.Scene.SceneTransitionStep;

namespace Lighthouse.Core.Scene.SceneTransitionPhase
{
    public sealed class LoadSceneGroupPhase : ISceneTransitionPhase
    {
        ISceneTransitionStep[] ISceneTransitionPhase.Steps { get; } =
        {
            new LoadSceneGroupStep(),
        };

        bool ISceneTransitionPhase.CanTransitionIntercept => false;
    }
}