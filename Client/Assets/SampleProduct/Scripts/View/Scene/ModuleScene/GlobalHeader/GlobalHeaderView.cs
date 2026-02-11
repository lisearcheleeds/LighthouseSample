using UnityEngine;
using UnityEngine.UI;

namespace SampleProduct.View.Scene.ModuleScene.GlobalHeader
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