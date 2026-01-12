using Lighthouse.Core.Scene.SceneTransitionStep;

namespace Lighthouse.Core.Scene.SceneTransitionPhase
{
    public sealed class LoadScenePhase : ISceneTransitionPhase
    {
        ISceneTransitionStep[] ISceneTransitionPhase.Steps { get; } =
        {
            new LoadMainSceneStep(), new LoadCommonSceneStep(),
        };

        bool ISceneTransitionPhase.CanTransitionIntercept => false;
    }
}