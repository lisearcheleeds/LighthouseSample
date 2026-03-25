using System.IO;
using Lighthouse.Editor.PropertyDrawer;
using UnityEditor;
using UnityEngine;

namespace LighthouseExtends.ScreenStack.Editor.ScriptableObject
{
    public class ScreenStackGenerateSettings : UnityEngine.ScriptableObject
    {
        [SerializeField, FolderOnly] DefaultAsset screenStackEntityFactoryDirectoryAsset;
        [SerializeField] string screenStackEntityFactoryClassName = "ScreenStackEntityFactory";
        [SerializeField] string screenStackEntityFactoryNamespace = "";

        string ScreenStackEntityFactoryDirectory
        {
            get
            {
                if (screenStackEntityFactoryDirectoryAsset == null) return string.Empty;
                var assetPath = AssetDatabase.GetAssetPath(screenStackEntityFactoryDirectoryAsset);
                return assetPath.StartsWith("Assets/") ? assetPath.Substring("Assets/".Length) : assetPath;
            }
        }

        public string ScreenStackEntityFactoryFilePath =>
            string.IsNullOrEmpty(ScreenStackEntityFactoryDirectory)
                ? string.Empty
                : Path.Combine(Application.dataPath, ScreenStackEntityFactoryDirectory, $"{screenStackEntityFactoryClassName}.g.cs");

        public string ScreenStackEntityFactoryClassName => screenStackEntityFactoryClassName;
        public string ScreenStackEntityFactoryNamespace => screenStackEntityFactoryNamespace;
    }
}