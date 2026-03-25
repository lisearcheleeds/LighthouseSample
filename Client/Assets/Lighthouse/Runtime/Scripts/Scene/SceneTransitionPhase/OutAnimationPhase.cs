using Lighthouse.Scene.SceneTransitionStep;

namespace Lighthouse.Scene.SceneTransitionPhase
{
    public sealed class OutAnimationPhase : ISceneTransitionPhase
    {
        ISceneTransitionStep[] ISceneTransitionPhase.Steps { get; } =
        {
            new OutAnimationStep(),
        };

        bool ISceneTransitionPhase.CanTransitionIntercept => false;
    }
}