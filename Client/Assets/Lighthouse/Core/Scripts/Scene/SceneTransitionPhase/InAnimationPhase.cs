using Lighthouse.Core.Scene.SceneTransitionStep;

namespace Lighthouse.Core.Scene.SceneTransitionPhase
{
    public sealed class InAnimationPhase : ISceneTransitionPhase
    {
        ISceneTransitionStep[] ISceneTransitionPhase.Steps { get; } =
        {
            new InAnimationStep(),
        };

        bool ISceneTransitionPhase.CanTransitionIntercept => false;
    }
}