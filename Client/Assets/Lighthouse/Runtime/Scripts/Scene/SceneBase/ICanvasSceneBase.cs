using Lighthouse.Scene.SceneCamera;

namespace Lighthouse.Scene.SceneBase
{
    public interface ICanvasSceneBase
    {
        ISceneCamera[] GetSceneCameraList();
        void InitializeCanvas(ISceneCamera canvasCamera);
    }
}