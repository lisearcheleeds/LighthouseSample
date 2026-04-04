using UnityEngine.EventSystems;

namespace Lighthouse.Scene.SceneCamera
{
    public interface ICanvasSceneObject
    {
        ISceneCamera UICamera { get; }
        EventSystem UIEventSystem { get; }
    }
}