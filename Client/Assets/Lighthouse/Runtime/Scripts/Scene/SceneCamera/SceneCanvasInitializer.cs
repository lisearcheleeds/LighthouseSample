using UnityEngine;

namespace Lighthouse.Scene.SceneCamera
{
    public class SceneCanvasInitializer : MonoBehaviour
    {
        [SerializeField] Canvas[] sceneCanvasList;

        void Awake()
        {
            foreach (var canvas in sceneCanvasList)
            {
                canvas.enabled = false;
            }
        }

        public void Initialize(ISceneCamera canvasCamera)
        {
            foreach (var canvas in sceneCanvasList)
            {
                canvas.enabled = true;
                canvas.renderMode = RenderMode.ScreenSpaceCamera;
                canvas.worldCamera = canvasCamera.GetCamera();
            }
        }
    }
}