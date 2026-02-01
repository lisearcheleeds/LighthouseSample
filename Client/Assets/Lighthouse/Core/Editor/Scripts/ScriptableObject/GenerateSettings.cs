using System.IO;
using UnityEngine;

namespace Lighthouse.Core.ScriptableObject
{
    public class GenerateSettings : UnityEngine.ScriptableObject
    {
        const string MainSceneIdFileName = "MainSceneId.cs";
        const string CommonSceneIdFileName = "CommonSceneId.cs";

        [SerializeField] string productNameSpace = "ProductNameSpace";
        [SerializeField] string generatedFileDirectory = "Product/Scripts/LighthouseGenerated";

        public string ProductNameSpace => productNameSpace;

        public string GeneratedFileDirectory => generatedFileDirectory;
        public string MainSceneIdFilePath => Path.Combine(Application.dataPath, GeneratedFileDirectory, MainSceneIdFileName);
        public string CommonSceneIdFilePath => Path.Combine(Application.dataPath, GeneratedFileDirectory, CommonSceneIdFileName);
    }
}