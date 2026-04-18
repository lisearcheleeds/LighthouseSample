using UnityEngine.InputSystem;

namespace LighthouseExtends.InputLayer
{
    public interface IInputLayer
    {
        bool BlocksAllInput { get; }

        bool OnActionStarted(InputAction action);

        bool OnActionPerformed(InputAction action);

        bool OnActionCanceled(InputAction action);
    }
}
