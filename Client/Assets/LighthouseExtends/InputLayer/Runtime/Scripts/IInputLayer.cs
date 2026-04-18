using UnityEngine.InputSystem;

namespace LighthouseExtends.InputLayer
{
    public interface IInputLayer
    {
        bool BlocksAllInput { get; }

        bool OnActionStarted(InputAction.CallbackContext callbackContext);

        bool OnActionPerformed(InputAction.CallbackContext callbackContext);

        bool OnActionCanceled(InputAction.CallbackContext callbackContext);
    }
}
