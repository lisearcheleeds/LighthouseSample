using Lighthouse.Scene.SceneCamera;

namespace Lighthouse.Scene
{
    public interface ISceneTransitionContext
    {
        public TransitionDataBase TransitionData { get; }
        public TransitionDirectionType TransitionDirectionType { get; }
        public TransitionType TransitionType { get; }
        public SceneTransitionDiff SceneTransitionDiff { get; }

        public IMainSceneManager MainSceneManager { get; }
        public IModuleSceneManager ModuleSceneManager { get; }
        public ISceneCameraManager SceneCameraManager { get; }
    }
}
