using Lighthouse.Core.Scene;
using UnityEngine.EventSystems;

namespace Product.Util
{
    public interface ICanvasSceneObject
    {
        ISceneCamera UICamera { get; }
        EventSystem UIEventSystem { get; }
    }
}