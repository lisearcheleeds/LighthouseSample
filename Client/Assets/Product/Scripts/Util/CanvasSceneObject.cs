using Lighthouse.Core.Scene.SceneCamera;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Product.Util
{
    public class CanvasSceneObject : MonoBehaviour, ICanvasSceneObject
    {
        [SerializeField] SceneCamera uiCamera;
        [SerializeField] EventSystem uiEventSystem;

        public ISceneCamera UICamera => uiCamera;
        public EventSystem UIEventSystem => uiEventSystem;
    }
}