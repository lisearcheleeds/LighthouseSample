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

        public virtual bool BlocksAllInput => true;

        public virtual bool OnActionStarted(InputAction action)
        {
            if (action.id == screenStackActions.Back.id && onBack != null)
            {
                onBack();
                return true;
            }

            return false;
        }

        public virtual bool OnActionPerformed(InputAction action)
        {
            return false;
        }

        public virtual bool OnActionCanceled(InputAction action)
        {
            return false;
        }
    }
}
