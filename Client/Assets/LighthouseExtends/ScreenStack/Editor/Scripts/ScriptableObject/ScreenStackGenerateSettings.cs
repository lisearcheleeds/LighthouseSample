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

        [Header("ScreenStack Dialog Script Generation")]
        [SerializeField, FolderOnly] DefaultAsset screenStackDialogScriptOutputDirectoryAsset;
        [SerializeField] string screenStackDialogScriptNamespace = "";

        [Header("ScreenStack Dialog Script Templates")]
        [SerializeField] TextAsset screenStackDialogDialogTemplate;
        [SerializeField] TextAsset screenStackDialogDataTemplate;
        [SerializeField] TextAsset screenStackDialogPresenterTemplate;
        [SerializeField] TextAsset screenStackDialogViewTemplate;

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

        public string ScreenStackDialogScriptOutputDirectory
        {
            get
            {
                if (screenStackDialogScriptOutputDirectoryAsset == null) return string.Empty;
                return AssetDatabase.GetAssetPath(screenStackDialogScriptOutputDirectoryAsset);
            }
        }

        public string ScreenStackDialogScriptNamespace => screenStackDialogScriptNamespace;
        public TextAsset ScreenStackDialogDialogTemplate => screenStackDialogDialogTemplate;
        public TextAsset ScreenStackDialogDataTemplate => screenStackDialogDataTemplate;
        public TextAsset ScreenStackDialogPresenterTemplate => screenStackDialogPresenterTemplate;
        public TextAsset ScreenStackDialogViewTemplate => screenStackDialogViewTemplate;

        void Reset()
        {
            InitializeDialogTemplateDefaults();
        }

        public void InitializeDialogTemplateDefaults()
        {
            const string TemplatesRoot = "Assets/LighthouseExtends/ScreenStack/Editor/Scripts/ScriptGenerator/DefaultScreenStackDialogTemplates";
            screenStackDialogDialogTemplate = AssetDatabase.LoadAssetAtPath<TextAsset>($"{TemplatesRoot}/Dialog.txt");
            screenStackDialogDataTemplate = AssetDatabase.LoadAssetAtPath<TextAsset>($"{TemplatesRoot}/Data.txt");
            screenStackDialogPresenterTemplate = AssetDatabase.LoadAssetAtPath<TextAsset>($"{TemplatesRoot}/Presenter.txt");
            screenStackDialogViewTemplate = AssetDatabase.LoadAssetAtPath<TextAsset>($"{TemplatesRoot}/View.txt");
        }
    }
}