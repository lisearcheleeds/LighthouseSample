using System;
using System.Linq;
using LighthouseExtends.ScreenStack.Editor.ScriptableObject;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

namespace LighthouseExtends.ScreenStack.Editor.ScriptGenerator
{
    public enum ScreenStackTemplateType
    {
        Dialog,
    }

    public class ScreenStackDialogScriptGeneratorWindow : EditorWindow
    {
        ScreenStackTemplateType templateType = ScreenStackTemplateType.Dialog;
        ScreenStackDialogBaseClassInfo[] baseClasses = Array.Empty<ScreenStackDialogBaseClassInfo>();
        string[] baseClassLabels = Array.Empty<string>();
        string screenStackName = string.Empty;
        bool hasCompileErrors;
        int selectedBaseClassIndex;
        ScreenStackGenerateSettings settings;
        string templateError;

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

        [MenuItem("Lighthouse/Generate/ScreenStack scripts from template")]
        static void Open()
        {
            var window = GetWindow<ScreenStackDialogScriptGeneratorWindow>("ScreenStack Script Generator");
            window.minSize = new Vector2(440, 250);
            window.Show();
        }

        void RefreshState()
        {
            if (ScreenStackDialogScriptGenerator.IsCompiling)
            {
                hasCompileErrors = false;
                templateError = null;
                return;
            }

            hasCompileErrors = ScreenStackDialogScriptGenerator.HasCompileErrors();
            if (hasCompileErrors)
            {
                templateError = null;
                return;
            }

            settings = LoadSettings();
            templateError = settings != null
                ? GetTemplateValidationError(settings)
                : "Failed to load ScreenStackGenerateSettings.";

            RefreshBaseClasses();
        }

        void RefreshBaseClasses()
        {
            baseClasses = ScreenStackDialogScriptGenerator.CollectBaseClasses();
            baseClassLabels = baseClasses.Select(b => b.DropdownLabel).ToArray();
            selectedBaseClassIndex = Mathf.Clamp(selectedBaseClassIndex, 0, Mathf.Max(0, baseClasses.Length - 1));
        }

        void OnGUI()
        {
            GUILayout.Label("ScreenStack Script Generator", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            if (ScreenStackDialogScriptGenerator.IsCompiling)
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
                if (GUILayout.Button("Open ScreenStackGenerateSettings"))
                {
                    ShowSettings();
                }

                return;
            }

            // Template Type
            EditorGUI.BeginChangeCheck();
            templateType = (ScreenStackTemplateType)EditorGUILayout.EnumPopup("Template Type", templateType);
            if (EditorGUI.EndChangeCheck())
            {
                templateError = GetTemplateValidationError(settings);
            }

            EditorGUILayout.Space();

            // Name
            EditorGUILayout.LabelField("Name");
            screenStackName = EditorGUILayout.TextField(screenStackName);
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

            if (string.IsNullOrWhiteSpace(screenStackName))
            {
                EditorGUILayout.HelpBox("Please enter a Name.", MessageType.Warning);
            }

            using (new EditorGUI.DisabledScope(
                       string.IsNullOrWhiteSpace(screenStackName) || baseClasses.Length == 0))
            {
                if (GUILayout.Button("Generate", GUILayout.Height(32)))
                {
                    Generate();
                }
            }
        }

        void Generate()
        {
            switch (templateType)
            {
                case ScreenStackTemplateType.Dialog:
                    ScreenStackDialogScriptGenerator.GenerateFiles(
                        screenStackName.Trim(),
                        baseClasses[selectedBaseClassIndex],
                        settings);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        string GetTemplateValidationError(ScreenStackGenerateSettings s)
        {
            switch (templateType)
            {
                case ScreenStackTemplateType.Dialog:
                    return ScreenStackDialogScriptGenerator.GetTemplateValidationError(s);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        static void ShowSettings()
        {
            var s = LoadSettings();
            if (s != null)
            {
                Selection.activeObject = s;
            }
        }

        static ScreenStackGenerateSettings LoadSettings()
        {
            var guids = AssetDatabase.FindAssets($"t:{nameof(ScreenStackGenerateSettings)}");
            if (guids.Length == 0)
            {
                return null;
            }

            if (guids.Length > 1)
            {
                Debug.LogWarning(
                    $"[ScreenStackDialogScriptGeneratorWindow] Multiple {nameof(ScreenStackGenerateSettings)} found. Using the first one.");
            }

            var path = AssetDatabase.GUIDToAssetPath(guids[0]);
            return (ScreenStackGenerateSettings)AssetDatabase.LoadAssetAtPath(path, typeof(ScreenStackGenerateSettings));
        }
    }
}
