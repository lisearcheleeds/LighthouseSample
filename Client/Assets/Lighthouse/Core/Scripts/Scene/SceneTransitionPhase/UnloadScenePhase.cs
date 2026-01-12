using Lighthouse.Core.Scene.SceneTransitionStep;

namespace Lighthouse.Core.Scene.SceneTransitionPhase
{
    public sealed class UnloadScenePhase : ISceneTransitionPhase
    {
        ISceneTransitionStep[] ISceneTransitionPhase.Steps { get; } =
        {
            new UnloadMainSceneStep(), new UnloadCommonSceneStep()
        };

        bool ISceneTransitionPhase.CanTransitionIntercept => false;
    }
}