using System;
using UnityEngine.InputSystem;

namespace SampleProduct.Input
{
    public class PurposeSceneInputLayer : SceneInputLayerBase
    {
        readonly Action onBack;

        public PurposeSceneInputLayer(Action onBack)
        {
            this.onBack = onBack;
        }

        public override void OnActionStarted(InputAction action)
        {
            if (action.name == InputActionNames.Back)
            {
                onBack?.Invoke();
            }
        }
    }
}
