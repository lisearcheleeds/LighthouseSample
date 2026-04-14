using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace SampleProduct.Input
{
    /// <summary>
    /// PurposeScene用InputLayer。
    /// Back でひとつ前のSceneに戻る。
    /// return false で下位レイヤーにも処理を渡す。
    /// </summary>
    public class PurposeSceneInputLayer : SceneInputLayerBase
    {
        readonly Action onBack;

        public PurposeSceneInputLayer(Action onBack)
        {
            this.onBack = onBack;
        }

        public override bool UpdateInput(HashSet<InputControl> consumedControls)
        {
            if (WasPressedThisFrame(InputActionNames.Back, consumedControls))
            {
                onBack?.Invoke();
            }
            return false;
        }
    }
}
