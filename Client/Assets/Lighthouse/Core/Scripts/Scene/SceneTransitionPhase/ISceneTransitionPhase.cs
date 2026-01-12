using Lighthouse.Core.Scene.SceneTransitionStep;

namespace Lighthouse.Core.Scene.SceneTransitionPhase
{
    public interface ISceneTransitionPhase
    {
        ISceneTransitionStep[] Steps { get; }

        bool CanTransitionIntercept { get; }
    }
}