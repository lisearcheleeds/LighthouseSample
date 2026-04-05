using System.Linq;
using Lighthouse.Editor.Menu;
using Lighthouse.Editor.ScriptableObject;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

namespace Lighthouse.Editor.ScriptGenerator
{
    public class SceneScriptGeneratorWindow : EditorWindow
    {
        string sceneName = string.Empty;
        SceneType sceneType = SceneType.MainScene;
        int selectedBaseClassIndex;
        BaseClassInfo[] baseClasses = System.Array.Empty<BaseClassInfo>();
        string[] baseClassLabels = System.Array.Empty<string>();
        bool hasCompileErrors;
        GenerateSettings settings;
        string templateError;

        [MenuItem("Lighthouse/Generate/Scene scripts from template")]
        static void Open()
        {
            var window = GetWindow<SceneScriptGeneratorWindow>("Scene Script Generator");
            window.minSize = new Vector2(440, 270);
            window.Show();
        }

        void OnEnable()
        {
            CompilationPipeline.compilationFinished += OnCompilationFinished;
            RefreshState();
        }

        void OnDisable()
        {
            CompilationPipeline.compilationFinished -= OnCompilationFinished;
        }

        void OnFocus()
        {
            RefreshState();
        }

        void OnCompilationFinished(object _)
        {
            RefreshState();
            Repaint();
        }

        void RefreshState()
        {
            if (SceneScriptGenerator.IsCompiling)
            {
                hasCompileErrors = false;
                templateError = null;
                return;
            }

            hasCompileErrors = SceneScriptGenerator.HasCompileErrors();
            if (hasCompileErrors)
            {
                templateError = null;
                return;
            }

            settings = LighthouseEditor.GetOrCreateSettings<GenerateSettings>();
            templateError = settings != null
                ? SceneScriptGenerator.GetTemplateValidationError(settings)
                : "Failed to load GenerateSettings.";
            RefreshBaseClasses();
        }

        void RefreshBaseClasses()
        {
            baseClasses = SceneScriptGenerator.CollectBaseClasses(sceneType);
            baseClassLabels = baseClasses.Select(b => b.DropdownLabel).ToArray();
            selectedBaseClassIndex = FindDefaultIndex();
        }

        int FindDefaultIndex()
        {
            var defaultName = sceneType == SceneType.MainScene ? "CanvasMainSceneBase" : "CanvasModuleSceneBase";
            var idx = System.Array.FindIndex(baseClasses, b => b.TypeName == defaultName);
            return idx >= 0 ? idx : 0;
        }

        void OnGUI()
        {
            GUILayout.Label("Scene Script Generator", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            if (SceneScriptGenerator.IsCompiling)
            {
                EditorGUILayout.HelpBox("Compiling. Please wait until compilation is finished.", MessageType.Info);
                return;
            }

            if (hasCompileErrors)
            {
                EditorGUILayout.HelpBox(
                    "Compile errors detected.\nPlease fix all errors and reopen this window.",
                    MessageType.Error);
                if (GUILayout.Button("Refresh"))
                {
                    RefreshState();
                }

                return;
            }

            if (templateError != null)
            {
                EditorGUILayout.HelpBox(templateError, MessageType.Error);
                if (GUILayout.Button("Open GenerateSettings"))
                {
                    LighthouseEditor.ShowGenerateSettings();
                }

                return;
            }

            // Scene Name
            EditorGUILayout.LabelField("Scene Name");
            sceneName = EditorGUILayout.TextField(sceneName);
            EditorGUILayout.Space();

            // Scene Type
            EditorGUI.BeginChangeCheck();
            sceneType = (SceneType)EditorGUILayout.EnumPopup("Scene Type", sceneType);
            if (EditorGUI.EndChangeCheck())
            {
                RefreshBaseClasses();
            }

            EditorGUILayout.Space();

            // Base Class
            if (baseClasses.Length > 0)
            {
                EditorGUILayout.LabelField("Base Class");
                selectedBaseClassIndex = EditorGUILayout.Popup(
                    Mathf.Clamp(selectedBaseClassIndex, 0, baseClasses.Length - 1),
                    baseClassLabels);
            }
            else
            {
                EditorGUILayout.HelpBox("No base classes found.", MessageType.Warning);
            }

            EditorGUILayout.Space(12);

            if (string.IsNullOrWhiteSpace(sceneName))
            {
                EditorGUILayout.HelpBox("Please enter a Scene Name.", MessageType.Warning);
            }

            using (new EditorGUI.DisabledScope(
                       string.IsNullOrWhiteSpace(sceneName) || baseClasses.Length == 0))
            {
                if (GUILayout.Button("Generate", GUILayout.Height(32)))
                {
                    Generate();
                }
            }
        }

        void Generate()
        {
            SceneScriptGenerator.GenerateFiles(
                sceneName.Trim(),
                sceneType,
                baseClasses[selectedBaseClassIndex],
                settings);
        }
    }
}
