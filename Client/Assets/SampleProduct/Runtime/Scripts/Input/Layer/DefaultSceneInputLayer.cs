using System;
using LighthouseExtends.InputLayer;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SampleProduct.Input.Layer
{
    public class DefaultSceneInputLayer : IInputLayer
    {
        readonly InputActions.SceneActions sceneActions;
        readonly Action onBack;

        public DefaultSceneInputLayer(InputActions actions, Action onBack)
        {
            sceneActions = actions.Scene;
            this.onBack = onBack;
        }

        public virtual bool BlocksAllInput => false;

        public virtual bool OnActionStarted(InputAction action)
        {
            if (action.id == sceneActions.Back.id && onBack != null)
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
