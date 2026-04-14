using LighthouseExtends.TextTable;
using LighthouseExtends.UIComponent.TextMeshPro;
using UnityEngine;

namespace SampleProduct.View.Scene.ModuleScene.GlobalHeader
{
    public class GlobalHeaderView : MonoBehaviour
    {
        [SerializeField] LHTextMeshPro text;

        public void SetText(ITextData textValue)
        {
            text.SetTextData(textValue);
        }
    }
}
