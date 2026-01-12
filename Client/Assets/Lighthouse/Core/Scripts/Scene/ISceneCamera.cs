using Lighthouse.Core.Scene.SceneCamera;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Lighthouse.Core.Scene
{
    public interface ISceneCamera
    {
        SceneCameraType SceneCameraType { get; }
        float CameraDefaultDepth { get; }

        void SetupCamera(CameraRenderType cameraRenderType, float runtimeDepth);
        void AddStackCamera(ISceneCamera overlaySceneCamera);
        void ClearStackCamera();

        public Camera GetCamera();
    }
}