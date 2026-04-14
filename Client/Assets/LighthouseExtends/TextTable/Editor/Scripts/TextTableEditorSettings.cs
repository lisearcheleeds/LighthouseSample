using UnityEngine;

namespace LighthouseExtends.TextTable.Editor
{
    public class TextTableEditorSettings : ScriptableObject
    {
        [SerializeField] string textTableFolderPath = "Assets/StreamingAssets/TextTables";

        public string TextTableFolderPath => textTableFolderPath;
    }
}
