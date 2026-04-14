using UnityEngine.InputSystem;

namespace LighthouseExtends.InputLayer
{
    public interface IInputLayerController
    {
        void PushLayer(InputLayer layer);
        void PopLayer();
        void PopLayer(InputLayer target);
        InputAction GetAction(string actionName);
    }
}
