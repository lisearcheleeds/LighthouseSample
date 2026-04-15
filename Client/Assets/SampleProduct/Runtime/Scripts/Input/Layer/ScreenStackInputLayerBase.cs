using LighthouseExtends.InputLayer;
using UnityEngine.InputSystem;

namespace SampleProduct.Input
{
    public abstract class ScreenStackInputLayerBase : IInputLayer
    {
        public virtual void OnActionStarted(InputAction action)
        {
        }

        public virtual void OnActionCanceled(InputAction action)
        {
        }
    }
}
