using Lighthouse.Core.Scene.SceneTransitionStep;

namespace Lighthouse.Core.Scene.SceneTransitionPhase
{
    public sealed class EnterScenePhase : ISceneTransitionPhase
    {
        ISceneTransitionStep[] ISceneTransitionPhase.Steps { get; } =
        {
            new EnterSceneStep(), new ResolveCameraStep(),
        };

        bool ISceneTransitionPhase.CanTransitionIntercept => false;
    }
}