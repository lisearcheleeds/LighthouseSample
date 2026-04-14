using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace SampleProduct.Input
{
    /// <summary>
    /// RequireToolsDialog用InputLayer。
    /// Back/Cancel でダイアログを閉じる。
    /// return true で下位レイヤー（Scene等）への伝播を止める。
    /// </summary>
    public class RequireToolsInputLayer : ScreenStackInputLayerBase
    {
        readonly Action onClose;

        public RequireToolsInputLayer(Action onClose)
        {
            this.onClose = onClose;
        }

        public override bool UpdateInput(HashSet<InputControl> consumedControls)
        {
            if (WasPressedThisFrame(InputActionNames.Back, consumedControls)
                || WasPressedThisFrame(InputActionNames.Cancel, consumedControls))
            {
                onClose?.Invoke();
            }
            return true;
        }
    }
}
