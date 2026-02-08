using System.IO;
using Lighthouse.Core.Editor.ScriptableObject;
using UnityEditor;
using UnityEngine;

namespace Lighthouse.Core.Editor
{
    public class LighthouseEditor : UnityEditor.Editor
    {
        static readonly string ApplicationRootDirectoryName = "Assets";
        static readonly string SettingsDirectoryParentPath = "Lighthouse/Core/Editor";
        static readonly string SettingsDirectoryName = "Settings";
        const string SettingsFileExtension = ".asset";

        [MenuItem("Lighthouse/Settings/GenerateSettings")]
        public static void ShowGenerateSettings()
        {
            ShowSettings<GenerateSettings>();
        }

        [MenuItem("Lighthouse/Settings/SceneEditSettings")]
        public static void ShowSceneEditSettings()
        {
            ShowSettings<SceneEditSettings>();
        }

        public static T GetSettings<T>() where T : UnityEngine.ScriptableObject
        {
            return LoadSettings<T>();
        }

        static void ShowSettings<T>() where T : UnityEngine.ScriptableObject
        {
            var instance = LoadSettings<T>();
            if (instance == null)
            {
                instance = CreateSettings<T>();
            }

            if (instance == null)
            {
                return;
            }

            Selection.activeObject = instance;
        }

        static T CreateSettings<T>() where T : UnityEngine.ScriptableObject
        {
            var settings = (T) CreateInstance(typeof(T));
            if (settings == null)
            {
                return null;
            }

            if (!Directory.Exists(Path.Combine(Application.dataPath, Path.Combine(SettingsDirectoryParentPath, SettingsDirectoryName))))
            {
                AssetDatabase.CreateFolder(Path.Combine(ApplicationRootDirectoryName, SettingsDirectoryParentPath), SettingsDirectoryName);
            }

            var settingsFilePath = Path.Combine(
                ApplicationRootDirectoryName,
                SettingsDirectoryParentPath,
                SettingsDirectoryName,
                $"{typeof(T).Name}{SettingsFileExtension}");
            AssetDatabase.CreateAsset(settings, settingsFilePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return settings;
        }

        static T LoadSettings<T>() where T : UnityEngine.ScriptableObject
        {
            var settingsFilePath = Path.Combine(
                ApplicationRootDirectoryName,
                SettingsDirectoryParentPath,
                SettingsDirectoryName,
                $"{typeof(T).Name}{SettingsFileExtension}");
            return (T) AssetDatabase.LoadAssetAtPath(Path.Combine(settingsFilePath), typeof(T));
        }
    }
}