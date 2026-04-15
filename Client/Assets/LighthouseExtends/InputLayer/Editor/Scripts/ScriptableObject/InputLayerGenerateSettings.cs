using System.IO;
using Lighthouse.Editor.PropertyDrawer;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LighthouseExtends.InputLayer.Editor.ScriptableObject
{
    public class InputLayerGenerateSettings : UnityEngine.ScriptableObject
    {
        [SerializeField] [FolderOnly] DefaultAsset outputDirectoryAsset;
        [SerializeField] InputActionAsset inputActionAsset;
        [SerializeField] string namespaceName = "";
        [SerializeField] string className = "InputActionNames";

        string OutputDirectory
        {
            get
            {
                if (outputDirectoryAsset == null)
                {
                    return string.Empty;
                }

                var assetPath = AssetDatabase.GetAssetPath(outputDirectoryAsset);
                return assetPath.StartsWith("Assets/") ? assetPath.Substring("Assets/".Length) : assetPath;
            }
        }

        public string OutputFilePath =>
            string.IsNullOrEmpty(OutputDirectory)
                ? string.Empty
                : Path.Combine(Application.dataPath, OutputDirectory, $"{className}.cs");

        public InputActionAsset InputActionAsset => inputActionAsset;
        public string NamespaceName => namespaceName;
        public string ClassName => className;
    }
}
