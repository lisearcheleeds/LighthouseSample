using System.IO;
using UnityEngine;

namespace Lighthouse.Editor.ScriptableObject
{
    public class GenerateSettings : UnityEngine.ScriptableObject
    {
        [SerializeField] string productNameSpace = "SampleProduct";
        [SerializeField] string generatedFileDirectory = "SampleProduct/Scripts/LighthouseGenerated";
        [SerializeField] string mainSceneIdPrefix = "SampleProduct";
        [SerializeField] string moduleSceneIdPrefix = "SampleProduct";

        string MainSceneIdFileName => $"{mainSceneIdPrefix}MainSceneId.cs";
        string ModuleSceneIdFileName => $"{mainSceneIdPrefix}ModuleSceneId.cs";

        public string ProductNameSpace => productNameSpace;

        public string GeneratedFileDirectory => generatedFileDirectory;
        public string MainSceneIdFilePath => Path.Combine(Application.dataPath, GeneratedFileDirectory, MainSceneIdFileName);
        public string SceneModuleIdIdFilePath => Path.Combine(Application.dataPath, GeneratedFileDirectory, ModuleSceneIdFileName);
        public string MainSceneIdPrefix => mainSceneIdPrefix;
        public string ModuleSceneIdPrefix => moduleSceneIdPrefix;
    }
}