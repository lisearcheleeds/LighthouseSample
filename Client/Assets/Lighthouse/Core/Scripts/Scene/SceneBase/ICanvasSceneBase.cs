using Lighthouse.Core.Scene.SceneCamera;

namespace Lighthouse.Core.Scene.SceneBase
{
    public interface ICanvasSceneBase
    {
        ISceneCamera[] GetSceneCameraList();
        void InitializeCanvas(ISceneCamera canvasCamera);
    }
}