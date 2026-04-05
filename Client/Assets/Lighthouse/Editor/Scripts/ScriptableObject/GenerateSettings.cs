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
        [SerializeField] TextAsset mainSceneIdTemplate;
        [SerializeField] TextAsset moduleSceneIdTemplate;

        [Header("Scene Script Generation")]
        [SerializeField, FolderOnly] DefaultAsset sceneScriptOutputDirectoryAsset;

        [Header("Scene Script Templates")]
        [SerializeField] TextAsset lifetimeScopeMainSceneTemplate;
        [SerializeField] TextAsset lifetimeScopeModuleSceneTemplate;
        [SerializeField] TextAsset sceneMainSceneTemplate;
        [SerializeField] TextAsset sceneModuleSceneTemplate;
        [SerializeField] TextAsset iPresenterTemplate;
        [SerializeField] TextAsset presenterTemplate;
        [SerializeField] TextAsset iViewTemplate;
        [SerializeField] TextAsset viewTemplate;

        string MainSceneIdFileName => $"{mainSceneIdPrefix}MainSceneId.g.cs";
        string ModuleSceneIdFileName => $"{mainSceneIdPrefix}ModuleSceneId.g.cs";

        public string ProductNameSpace => productNameSpace;

        public string GeneratedFileDirectory
        {
            get
            {
                if (generatedFileDirectoryAsset == null)
                {
                    return string.Empty;
                }

                var assetPath = AssetDatabase.GetAssetPath(generatedFileDirectoryAsset);
                return assetPath.StartsWith("Assets/") ? assetPath.Substring("Assets/".Length) : assetPath;
            }
        }

        public string MainSceneIdFilePath => Path.Combine(Application.dataPath, GeneratedFileDirectory, MainSceneIdFileName);
        public string SceneModuleIdIdFilePath => Path.Combine(Application.dataPath, GeneratedFileDirectory, ModuleSceneIdFileName);
        public string MainSceneIdPrefix => mainSceneIdPrefix;
        public string ModuleSceneIdPrefix => moduleSceneIdPrefix;
        public TextAsset MainSceneIdTemplate => mainSceneIdTemplate;
        public TextAsset ModuleSceneIdTemplate => moduleSceneIdTemplate;

        public string GeneratedMainSceneIdClassName => $"{mainSceneIdPrefix}MainSceneId";
        public string GeneratedModuleSceneIdClassName => $"{moduleSceneIdPrefix}ModuleSceneId";
        public string GeneratedSceneIdNamespace => $"{productNameSpace}.LighthouseGenerated";

        public string SceneScriptOutputDirectory
        {
            get
            {
                if (sceneScriptOutputDirectoryAsset == null)
                {
                    return string.Empty;
                }

                return AssetDatabase.GetAssetPath(sceneScriptOutputDirectoryAsset);
            }
        }

        public TextAsset LifetimeScopeMainSceneTemplate => lifetimeScopeMainSceneTemplate;
        public TextAsset LifetimeScopeModuleSceneTemplate => lifetimeScopeModuleSceneTemplate;
        public TextAsset SceneMainSceneTemplate => sceneMainSceneTemplate;
        public TextAsset SceneModuleSceneTemplate => sceneModuleSceneTemplate;
        public TextAsset IPresenterTemplate => iPresenterTemplate;
        public TextAsset PresenterTemplate => presenterTemplate;
        public TextAsset IViewTemplate => iViewTemplate;
        public TextAsset ViewTemplate => viewTemplate;

        void Reset()
        {
            InitializeDefaults();
        }

        public void InitializeDefaults()
        {
            const string TemplatesRoot = "Assets/Lighthouse/Editor/Scripts/ScriptGenerator/DefaultSceneScriptTemplates";
            lifetimeScopeMainSceneTemplate = AssetDatabase.LoadAssetAtPath<TextAsset>($"{TemplatesRoot}/LifetimeScope_MainScene.txt");
            lifetimeScopeModuleSceneTemplate = AssetDatabase.LoadAssetAtPath<TextAsset>($"{TemplatesRoot}/LifetimeScope_ModuleScene.txt");
            sceneMainSceneTemplate = AssetDatabase.LoadAssetAtPath<TextAsset>($"{TemplatesRoot}/Scene_MainScene.txt");
            sceneModuleSceneTemplate = AssetDatabase.LoadAssetAtPath<TextAsset>($"{TemplatesRoot}/Scene_ModuleScene.txt");
            iPresenterTemplate = AssetDatabase.LoadAssetAtPath<TextAsset>($"{TemplatesRoot}/IPresenter.txt");
            presenterTemplate = AssetDatabase.LoadAssetAtPath<TextAsset>($"{TemplatesRoot}/Presenter.txt");
            iViewTemplate = AssetDatabase.LoadAssetAtPath<TextAsset>($"{TemplatesRoot}/IView.txt");
            viewTemplate = AssetDatabase.LoadAssetAtPath<TextAsset>($"{TemplatesRoot}/View.txt");
        }
    }
}
