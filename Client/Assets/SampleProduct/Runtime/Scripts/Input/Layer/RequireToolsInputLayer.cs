using System;
using UnityEngine.InputSystem;

namespace SampleProduct.Input
{
    public class RequireToolsInputLayer : ScreenStackInputLayerBase
    {
        readonly PlayerInputActions.DialogActions dialogActions;
        readonly Action onClose;

        public RequireToolsInputLayer(PlayerInputActions actions, Action onClose)
        {
            dialogActions = actions.Dialog;
            this.onClose = onClose;
        }

        public override void OnActionStarted(InputAction action)
        {
            if (action.id == dialogActions.Back.id || action.id == dialogActions.Cancel.id)
            {
                onClose?.Invoke();
            }
        }
    }
}
