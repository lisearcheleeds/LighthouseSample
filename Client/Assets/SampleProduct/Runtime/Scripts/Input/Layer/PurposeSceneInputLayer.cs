using System;
using UnityEngine.InputSystem;

namespace SampleProduct.Input.Layer
{
    public class PurposeSceneInputLayer : SceneInputLayerBase
    {
        readonly InputActions.SceneActions sceneActions;
        readonly Action onBack;

        public PurposeSceneInputLayer(InputActions actions, Action onBack)
        {
            sceneActions = actions.Scene;
            this.onBack = onBack;
        }

        public override void OnActionStarted(InputAction action)
        {
            if (action.id == sceneActions.Back.id)
            {
                onBack?.Invoke();
            }
        }
    }
}
