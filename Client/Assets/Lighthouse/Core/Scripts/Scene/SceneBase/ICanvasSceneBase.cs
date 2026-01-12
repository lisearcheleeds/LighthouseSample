namespace Lighthouse.Core.Scene
{
    public interface ICanvasSceneBase
    {
        ISceneCamera[] GetSceneCameraList();
        void InitializeCanvas(ISceneCamera canvasCamera);
    }
}