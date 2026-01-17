namespace Lighthouse.Core.Scene
{
    public interface ISceneCameraManager
    {
        ISceneCamera BaseCamera { get; }
        ISceneCamera UICamera { get; }

        ISceneCamera[] OverlayCameraList { get; }

        void UpdateCameraStack(MainSceneGroup afterMainSceneGroup, ICommonSceneManager commonSceneManager, CommonSceneKey[] targetCommonSceneIds);
    }
}