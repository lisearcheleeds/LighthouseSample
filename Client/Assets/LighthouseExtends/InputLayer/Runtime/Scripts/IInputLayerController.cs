using UnityEngine.InputSystem;

namespace LighthouseExtends.InputLayer
{
    public interface IInputLayerController
    {
        void SetGlobalLayer(IInputLayer layer, InputActionMap actionMap);
        void PushLayer(IInputLayer layer, InputActionMap actionMap);
        void PopLayer();
        void PopLayer(IInputLayer target);
    }
}
