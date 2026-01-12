using Lighthouse.Core.Scene.SceneTransitionStep;

namespace Lighthouse.Core.Scene.SceneTransitionPhase
{
    public sealed class LeaveScenePhase : ISceneTransitionPhase
    {
        ISceneTransitionStep[] ISceneTransitionPhase.Steps { get; } =
        {
            new LeaveSceneStep()
        };

        bool ISceneTransitionPhase.CanTransitionIntercept => false;
    }
}