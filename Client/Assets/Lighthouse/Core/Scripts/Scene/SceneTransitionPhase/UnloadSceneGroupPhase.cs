using Lighthouse.Core.Scene.SceneTransitionStep;

namespace Lighthouse.Core.Scene.SceneTransitionPhase
{
    public sealed class UnloadSceneGroupPhase : ISceneTransitionPhase
    {
        ISceneTransitionStep[] ISceneTransitionPhase.Steps { get; } =
        {
            new UnloadSceneGroupStep(),
        };

        bool ISceneTransitionPhase.CanTransitionIntercept => false;
    }
}