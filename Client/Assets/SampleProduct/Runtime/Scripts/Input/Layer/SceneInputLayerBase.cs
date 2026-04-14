using System.Collections.Generic;
using LighthouseExtends.InputLayer;
using UnityEngine;

namespace SampleProduct.Input
{
    /// <summary>
    /// Scene用InputLayer基底。
    /// Back/Cancel をデフォルトで消費する。
    /// return false で下位レイヤーにも処理を渡す想定。
    /// </summary>
    public abstract class SceneInputLayerBase : InputLayer
    {
        public override CursorLockMode CursorLockMode => CursorLockMode.None;

        protected override IReadOnlyList<string> ConsumedActions => new[]
        {
            InputActionNames.Back,
            InputActionNames.Cancel,
        };
    }
}
