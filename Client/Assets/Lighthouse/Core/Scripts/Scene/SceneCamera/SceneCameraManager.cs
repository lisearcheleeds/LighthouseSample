using System;
using System.Linq;
using Product.Util;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using VContainer;

namespace Lighthouse.Core.Scene
{
    public class SceneCameraManager : ISceneCameraManager
    {
        readonly ICanvasSceneObject canvasSceneObject;

        public ISceneCamera BaseCamera { get; private set; }
        public ISceneCamera UICamera => canvasSceneObject.UICamera;

        public ISceneCamera[] OverlayCameraList { get; private set; }

        [Inject]
        public SceneCameraManager(ICanvasSceneObject canvasSceneObject)
        {
            this.canvasSceneObject = canvasSceneObject;
        }

        void ISceneCameraManager.UpdateCameraStack(
            MainSceneGroup afterMainSceneGroup,
            CommonSceneManager commonSceneManager,
            CommonSceneKey[] targetCommonSceneIds)
        {
            var sceneCameras = commonSceneManager
                .GetSceneCameraList(targetCommonSceneIds)
                .Concat(afterMainSceneGroup.GetSceneCameraList() ?? Array.Empty<ISceneCamera>())
                .Concat(new[] { UICamera })
                .Distinct()
                .OrderBy(x => (x.SceneCameraType, x.CameraDefaultDepth)).ToArray()
                .ToArray();

            var baseCamera = sceneCameras.First();
            var overlayCameraList = sceneCameras.Skip(1).ToArray();

            if (ReferenceEquals(BaseCamera, baseCamera) && OverlayCameraList.SequenceEqual(overlayCameraList))
            {
                return;
            }

            if (BaseCamera != null)
            {
                BaseCamera.ClearStackCamera();
            }

            if (OverlayCameraList != null)
            {
                foreach (var overlayCamera in OverlayCameraList)
                {
                    overlayCamera.ClearStackCamera();
                }
            }

            BaseCamera = baseCamera;
            OverlayCameraList = overlayCameraList;

            var depth = 0.0f;
            BaseCamera.SetupCamera(CameraRenderType.Base, depth);

            foreach (var overlayCamera in overlayCameraList)
            {
                depth++;

                overlayCamera.SetupCamera(CameraRenderType.Overlay, depth);
                BaseCamera.AddStackCamera(overlayCamera);
            }
        }
    }
}