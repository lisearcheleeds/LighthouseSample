using System.Linq;
using UnityEngine;

namespace Lighthouse.Core.Scene
{
    public class SceneCanvasInitializer : MonoBehaviour
    {
        [SerializeField] Canvas[] sceneCanvasList;

        public void Initialize(ISceneCamera canvasCamera)
        {
            foreach (var canvas in sceneCanvasList.Where(c => c.worldCamera == null))
            {
                canvas.renderMode = RenderMode.ScreenSpaceCamera;
                canvas.worldCamera = canvasCamera.GetCamera();
            }
        }
    }
}