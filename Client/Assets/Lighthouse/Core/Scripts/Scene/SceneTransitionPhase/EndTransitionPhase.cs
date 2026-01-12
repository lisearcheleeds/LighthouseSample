using Lighthouse.Core.Scene.SceneTransitionStep;

namespace Lighthouse.Core.Scene.SceneTransitionPhase
{
    public sealed class EndTransitionPhase : ISceneTransitionPhase
    {
        ISceneTransitionStep[] ISceneTransitionPhase.Steps { get; } =
        {
            new CleanupStep(), new FinishedStep(),
        };

        bool ISceneTransitionPhase.CanTransitionIntercept => false;
    }
}