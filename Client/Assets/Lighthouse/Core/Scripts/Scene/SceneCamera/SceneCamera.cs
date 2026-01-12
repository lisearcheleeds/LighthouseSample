using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Lighthouse.Core.Scene.SceneCamera
{
    [RequireComponent(typeof(Camera))]
    public class SceneCamera : MonoBehaviour, ISceneCamera
    {
        [SerializeField] SceneCameraType sceneCameraType;
        [SerializeField] Camera bindCamera;

        float? defaultDepth;

        SceneCameraType ISceneCamera.SceneCameraType => sceneCameraType;

        float ISceneCamera.CameraDefaultDepth
        {
            get
            {
                defaultDepth ??= bindCamera.depth;
                return defaultDepth.Value;
            }
        }

        void ISceneCamera.SetupCamera(CameraRenderType cameraRenderType, float runtimeDepth)
        {
            bindCamera.gameObject.SetActive(true);
            bindCamera.GetUniversalAdditionalCameraData().renderType = cameraRenderType;

            defaultDepth ??= bindCamera.depth;
            bindCamera.depth = runtimeDepth;
        }

        void ISceneCamera.AddStackCamera(ISceneCamera overlaySceneCamera)
        {
            bindCamera.GetUniversalAdditionalCameraData().cameraStack.Add(overlaySceneCamera.GetCamera());
        }

        void ISceneCamera.ClearStackCamera()
        {
            bindCamera.GetUniversalAdditionalCameraData().cameraStack.Clear();
        }

        Camera ISceneCamera.GetCamera()
        {
            return bindCamera;
        }
    }
}