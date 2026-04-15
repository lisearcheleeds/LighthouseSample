using UnityEngine.InputSystem;

namespace LighthouseExtends.InputLayer
{
    public interface IInputLayer
    {
        void OnActionStarted(InputAction action);
        void OnActionCanceled(InputAction action);
    }
}
