using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace SampleProduct.Input
{
    /// <summary>
    /// HomeScene用InputLayer。
    /// HomeScene は最上位シーンのため Back での遷移は行わない。
    /// return false で下位レイヤーにも処理を渡す。
    /// </summary>
    public class HomeSceneInputLayer : SceneInputLayerBase
    {
        public override bool UpdateInput(HashSet<InputControl> consumedControls)
        {
            return false;
        }
    }
}
