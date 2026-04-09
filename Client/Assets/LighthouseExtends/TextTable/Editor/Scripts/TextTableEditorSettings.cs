using UnityEngine;

namespace LighthouseExtends.TextTable.Editor
{
    public class TextTableEditorSettings : ScriptableObject
    {
        [SerializeField] string textTableFolderPath = "Assets/SampleProduct/Runtime/Resources/TextTables";

        public string TextTableFolderPath => textTableFolderPath;
    }
}
