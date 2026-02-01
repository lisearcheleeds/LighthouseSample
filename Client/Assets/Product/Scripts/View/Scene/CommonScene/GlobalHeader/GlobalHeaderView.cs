using UnityEngine;
using UnityEngine.UI;

namespace Product.View.Scene.CommonScene.GlobalHeader
{
    public class GlobalHeaderView : MonoBehaviour
    {
        [SerializeField] Text text;

        public void SetText(string textValue)
        {
            text.text = textValue;
        }
    }
}