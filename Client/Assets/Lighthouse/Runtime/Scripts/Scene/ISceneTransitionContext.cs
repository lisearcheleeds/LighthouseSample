using Lighthouse.Scene.SceneCamera;

namespace Lighthouse.Scene
{
    public interface ISceneTransitionContext
    {
        TransitionDataBase TransitionData { get; }
        TransitionDirectionType TransitionDirectionType { get; }
        TransitionType TransitionType { get; }
        SceneTransitionDiff SceneTransitionDiff { get; }

        IMainSceneManager MainSceneManager { get; }
        IModuleSceneManager ModuleSceneManager { get; }
        ISceneCameraManager SceneCameraManager { get; }
    }
}
