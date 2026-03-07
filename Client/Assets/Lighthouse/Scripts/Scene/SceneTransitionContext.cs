using Lighthouse.Scene.SceneCamera;

namespace Lighthouse.Scene
{
    public class SceneTransitionContext
    {
        public TransitionDataBase TransitionData { get; }
        public TransitionType TransitionType { get; }
        public SceneTransitionDiff SceneTransitionDiff { get; }
        public IMainSceneManager MainSceneManager { get; }
        public IModuleSceneManager ModuleSceneManager { get; }
        public ISceneCameraManager SceneCameraManager { get; }

        public SceneTransitionContext(
            TransitionDataBase transitionData,
            TransitionType transitionType,
            SceneTransitionDiff sceneTransitionDiff,
            IMainSceneManager mainSceneManager,
            IModuleSceneManager moduleSceneManager,
            ISceneCameraManager sceneCameraManager)
        {
            TransitionData = transitionData;
            TransitionType = transitionType;
            SceneTransitionDiff = sceneTransitionDiff;
            MainSceneManager = mainSceneManager;
            ModuleSceneManager = moduleSceneManager;
            SceneCameraManager = sceneCameraManager;
        }
    }
}