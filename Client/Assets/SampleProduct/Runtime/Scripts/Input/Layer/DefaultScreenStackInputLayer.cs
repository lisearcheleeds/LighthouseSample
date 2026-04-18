using System;
using LighthouseExtends.InputLayer;
using UnityEngine.InputSystem;

namespace SampleProduct.Input.Layer
{
    public class DefaultScreenStackInputLayer : IInputLayer
    {
        readonly InputActions.ScreenStackActions screenStackActions;
        readonly Action onBack;

        public DefaultScreenStackInputLayer(InputActions actions, Action onBack)
        {
            screenStackActions = actions.ScreenStack;
            this.onBack = onBack;
        }

        public virtual void OnActionStarted(InputAction action)
        {
            if (action.id == screenStackActions.Back.id)
            {
                onBack?.Invoke();
            }
        }

        public virtual void OnActionPerformed(InputAction action)
        {
        }

        public virtual void OnActionCanceled(InputAction action)
        {
        }
    }
}
