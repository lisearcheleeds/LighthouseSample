using Lighthouse.Scene.SceneTransitionStep;

namespace Lighthouse.Scene.SceneTransitionPhase
{
    public sealed class FinishTransitionPhase : ISceneTransitionPhase
    {
        ISceneTransitionStep[] ISceneTransitionPhase.Steps { get; } =
        {
            new FinishStep(),
        };

        bool ISceneTransitionPhase.CanTransitionIntercept => false;
    }
}