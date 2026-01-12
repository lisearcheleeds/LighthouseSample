using Lighthouse.Core.Scene.SceneTransitionStep;

namespace Lighthouse.Core.Scene.SceneTransitionPhase
{
    public sealed class CrossAnimationPhase : ISceneTransitionPhase
    {
        ISceneTransitionStep[] ISceneTransitionPhase.Steps { get; } =
        {
            new InAnimationStep(), new OutAnimationStep(),
        };

        bool ISceneTransitionPhase.CanTransitionIntercept => false;
    }
}