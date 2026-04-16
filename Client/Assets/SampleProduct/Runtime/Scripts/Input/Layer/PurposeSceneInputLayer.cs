using System;
using UnityEngine.InputSystem;

namespace SampleProduct.Input
{
    public class PurposeSceneInputLayer : SceneInputLayerBase
    {
        readonly PlayerInputActions.SceneActions sceneActions;
        readonly Action onBack;

        public PurposeSceneInputLayer(PlayerInputActions actions, Action onBack)
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
