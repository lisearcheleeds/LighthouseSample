using System.Collections.Generic;
using LighthouseExtends.InputLayer;
using UnityEngine;

namespace SampleProduct.Input
{
    /// <summary>
    /// ScreenStack用InputLayer基底。
    /// ナビゲーション系アクションをすべて消費する。
    /// return true で下位レイヤー（Scene等）への伝播を止める想定。
    /// </summary>
    public abstract class ScreenStackInputLayerBase : InputLayer
    {
        public override CursorLockMode CursorLockMode => CursorLockMode.None;

        protected override IReadOnlyList<string> ConsumedActions => new[]
        {
            InputActionNames.Confirm,
            InputActionNames.Cancel,
            InputActionNames.Back,
            InputActionNames.NavigateUp,
            InputActionNames.NavigateDown,
            InputActionNames.NavigateLeft,
            InputActionNames.NavigateRight,
        };
    }
}
