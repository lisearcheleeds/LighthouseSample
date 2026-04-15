using System;
using UnityEngine.InputSystem;

namespace SampleProduct.Input
{
    public class RequireToolsInputLayer : ScreenStackInputLayerBase
    {
        readonly Action onClose;

        public RequireToolsInputLayer(Action onClose)
        {
            this.onClose = onClose;
        }

        public override void OnActionStarted(InputAction action)
        {
            if (action.name == InputActionNames.Back || action.name == InputActionNames.Cancel)
            {
                onClose?.Invoke();
            }
        }
    }
}
