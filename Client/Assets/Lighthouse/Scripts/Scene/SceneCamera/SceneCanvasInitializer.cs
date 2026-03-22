using UnityEngine;

namespace Lighthouse.Scene.SceneCamera
{
    public class SceneCanvasInitializer : MonoBehaviour
    {
        [SerializeField] Canvas[] sceneCanvasList;

        public void Initialize(ISceneCamera canvasCamera)
        {
            foreach (var canvas in sceneCanvasList)
            {
                canvas.renderMode = RenderMode.ScreenSpaceCamera;
                canvas.worldCamera = canvasCamera.GetCamera();
            }
        }
    }
}