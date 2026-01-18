namespace Lighthouse.Core.Scene.SceneCamera
{
    public interface ISceneCameraManager
    {
        ISceneCamera BaseCamera { get; }
        ISceneCamera UICamera { get; }

        ISceneCamera[] OverlayCameraList { get; }

        void UpdateCameraStack(MainSceneGroup afterMainSceneGroup, ICommonSceneManager commonSceneManager, CommonSceneKey[] targetCommonSceneIds);
    }
}