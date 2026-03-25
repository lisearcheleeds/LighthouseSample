using Lighthouse.Scene.SceneTransitionStep;

namespace Lighthouse.Scene.SceneTransitionPhase
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