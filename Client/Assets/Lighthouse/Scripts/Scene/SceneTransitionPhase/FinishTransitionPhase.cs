using Lighthouse.Scene.SceneTransitionStep;

namespace Lighthouse.Scene.SceneTransitionPhase
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