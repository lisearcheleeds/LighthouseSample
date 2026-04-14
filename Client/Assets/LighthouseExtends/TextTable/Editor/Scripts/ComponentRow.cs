using System.Collections.Generic;
using LighthouseExtends.UIComponent.TextMeshPro;

namespace LighthouseExtends.TextTable.Editor
{
    public class ComponentRow
    {
        public readonly LHTextMeshPro component;
        public readonly string hierarchyPath;
        public readonly Dictionary<string, string> langData;
        public readonly string placeholderText;
        string savedTextKey;
        public string textKey;

        public ComponentRow(string hierarchyPath, string textKey, string placeholderText,
            LHTextMeshPro component, Dictionary<string, string> langData)
        {
            this.hierarchyPath = hierarchyPath;
            this.textKey = textKey;
            savedTextKey = textKey;
            this.placeholderText = placeholderText;
            this.component = component;
            this.langData = langData;
        }

        public bool IsTextKeyDirty => textKey != savedTextKey;

        public void MarkSaved()
        {
            savedTextKey = textKey;
        }
    }
}
