namespace Lighthouse.Scene.SceneCamera
{
    public interface ISceneCameraManager
    {
        ISceneCamera BaseCamera { get; }
        ISceneCamera UICamera { get; }

        ISceneCamera[] OverlayCameraList { get; }

        void UpdateCameraStack(IMainSceneManager mainSceneManager, SceneTransitionDiff sceneTransitionDiff);
    }
}