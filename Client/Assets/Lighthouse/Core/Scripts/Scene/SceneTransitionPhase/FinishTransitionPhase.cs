using Lighthouse.Core.Scene.SceneTransitionStep;

namespace Lighthouse.Core.Scene.SceneTransitionPhase
{
    public sealed class FinishTransitionPhase : ISceneTransitionPhase
    {
        ISceneTransitionStep[] ISceneTransitionPhase.Steps { get; } =
        {
            new FinishStep(),
            new CleanupStep(),
        };

        bool ISceneTransitionPhase.CanTransitionIntercept => false;
    }
}