using System.IO;
using Lighthouse.Editor.PropertyDrawer;
using UnityEditor;
using UnityEngine;

namespace Lighthouse.Editor.ScriptableObject
{
    public class GenerateSettings : UnityEngine.ScriptableObject
    {
        [SerializeField] string productNameSpace = "SampleProduct";
        [SerializeField, FolderOnly] DefaultAsset generatedFileDirectoryAsset;
        [SerializeField] string mainSceneIdPrefix = "SampleProduct";
        [SerializeField] string moduleSceneIdPrefix = "SampleProduct";

        string MainSceneIdFileName => $"{mainSceneIdPrefix}MainSceneId.g.cs";
        string ModuleSceneIdFileName => $"{mainSceneIdPrefix}ModuleSceneId.g.cs";

        public string ProductNameSpace => productNameSpace;

        public string GeneratedFileDirectory
        {
            get
            {
                if (generatedFileDirectoryAsset == null) return string.Empty;
                var assetPath = AssetDatabase.GetAssetPath(generatedFileDirectoryAsset);
                return assetPath.StartsWith("Assets/") ? assetPath.Substring("Assets/".Length) : assetPath;
            }
        }

        public string MainSceneIdFilePath => Path.Combine(Application.dataPath, GeneratedFileDirectory, MainSceneIdFileName);
        public string SceneModuleIdIdFilePath => Path.Combine(Application.dataPath, GeneratedFileDirectory, ModuleSceneIdFileName);
        public string MainSceneIdPrefix => mainSceneIdPrefix;
        public string ModuleSceneIdPrefix => moduleSceneIdPrefix;
    }
}