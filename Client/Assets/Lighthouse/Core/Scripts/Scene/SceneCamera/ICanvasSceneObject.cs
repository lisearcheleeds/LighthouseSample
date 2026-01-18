using UnityEngine.EventSystems;

namespace Lighthouse.Core.Scene.SceneCamera
{
    public interface ICanvasSceneObject
    {
        ISceneCamera UICamera { get; }
        EventSystem UIEventSystem { get; }
    }
}