using System;
using LighthouseExtends.InputLayer;
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

        public virtual void OnActionStarted(InputAction action)
        {
            if (action.id == sceneActions.Back.id)
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
