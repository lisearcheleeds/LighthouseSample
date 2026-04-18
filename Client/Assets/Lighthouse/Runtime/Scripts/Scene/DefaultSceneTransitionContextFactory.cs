using Lighthouse.Scene.SceneCamera;

namespace Lighthouse.Scene
{
    public class DefaultSceneTransitionContextFactory : ISceneTransitionContextFactory
    {
        public ISceneTransitionContext Create(
            TransitionDataBase transitionData,
            TransitionDirectionType transitionDirectionType,
            TransitionType transitionType,
            SceneTransitionDiff sceneTransitionDiff,
            IMainSceneManager mainSceneManager,
            IModuleSceneManager moduleSceneManager,
            ISceneCameraManager sceneCameraManager)
        {
            return new SceneTransitionContext(
                transitionData,
                transitionDirectionType,
                transitionType,
                sceneTransitionDiff,
                mainSceneManager,
                moduleSceneManager,
                sceneCameraManager);
        }
    }
}
