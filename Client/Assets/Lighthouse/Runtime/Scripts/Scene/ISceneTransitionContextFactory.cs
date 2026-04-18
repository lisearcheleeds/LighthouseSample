using Lighthouse.Scene.SceneCamera;

namespace Lighthouse.Scene
{
    public interface ISceneTransitionContextFactory
    {
        ISceneTransitionContext Create(
            TransitionDataBase transitionData,
            TransitionDirectionType transitionDirectionType,
            TransitionType transitionType,
            SceneTransitionDiff sceneTransitionDiff,
            IMainSceneManager mainSceneManager,
            IModuleSceneManager moduleSceneManager,
            ISceneCameraManager sceneCameraManager);
    }
}
