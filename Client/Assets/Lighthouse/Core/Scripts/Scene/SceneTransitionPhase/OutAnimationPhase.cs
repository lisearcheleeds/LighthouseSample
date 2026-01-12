using Lighthouse.Core.Scene.SceneTransitionStep;

namespace Lighthouse.Core.Scene.SceneTransitionPhase
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