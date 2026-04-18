using System;
using UnityEngine.InputSystem;

namespace LighthouseExtends.InputLayer
{
    public interface IInputLayerController : IDisposable
    {
        void PushLayer(IInputLayer layer, InputActionMap actionMap);
        void PopLayer();
        void PopLayer(IInputLayer target);
    }
}
