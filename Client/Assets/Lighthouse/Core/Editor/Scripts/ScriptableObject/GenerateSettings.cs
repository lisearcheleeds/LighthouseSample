using System.IO;
using UnityEngine;

namespace Lighthouse.Core.Editor.ScriptableObject
{
    public class GenerateSettings : UnityEngine.ScriptableObject
    {
        [SerializeField] string productNameSpace = "SampleProduct";
        [SerializeField] string generatedFileDirectory = "SampleProduct/Scripts/LighthouseGenerated";
        [SerializeField] string mainSceneIdPrefix = "SampleProduct";
        [SerializeField] string sceneModuleIdPrefix = "SampleProduct";

        string MainSceneIdFileName => $"{mainSceneIdPrefix}MainSceneId.cs";
        string SceneModuleIdFileName => $"{mainSceneIdPrefix}SceneModuleId.cs";

        public string ProductNameSpace => productNameSpace;

        public string GeneratedFileDirectory => generatedFileDirectory;
        public string MainSceneIdFilePath => Path.Combine(Application.dataPath, GeneratedFileDirectory, MainSceneIdFileName);
        public string SceneModuleIdIdFilePath => Path.Combine(Application.dataPath, GeneratedFileDirectory, SceneModuleIdFileName);
        public string MainSceneIdPrefix => mainSceneIdPrefix;
        public string SceneModuleIdPrefix => sceneModuleIdPrefix;
    }
}