namespace Lighthouse.Core.Scene
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