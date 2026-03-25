using Lighthouse.Scene.SceneTransitionStep;

namespace Lighthouse.Scene.SceneTransitionPhase
{
    public interface ISceneTransitionPhase
    {
        ISceneTransitionStep[] Steps { get; }

        bool CanTransitionIntercept { get; }
    }
}