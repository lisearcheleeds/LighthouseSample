using System.IO;
using UnityEngine;

namespace LighthouseExtends.ScreenStack.Editor.ScriptableObject
{
    public class ScreenStackGenerateSettings : UnityEngine.ScriptableObject
    {
        [SerializeField] string screenStackEntityFactoryDirectory = "";
        [SerializeField] string screenStackEntityFactoryClassName = "ScreenStackEntityFactory";
        [SerializeField] string screenStackEntityFactoryNamespace = "";

        public string ScreenStackEntityFactoryFilePath =>
            string.IsNullOrEmpty(screenStackEntityFactoryDirectory)
                ? string.Empty
                : Path.Combine(Application.dataPath, screenStackEntityFactoryDirectory, $"{screenStackEntityFactoryClassName}.g.cs");

        public string ScreenStackEntityFactoryClassName => screenStackEntityFactoryClassName;
        public string ScreenStackEntityFactoryNamespace => screenStackEntityFactoryNamespace;
    }
}