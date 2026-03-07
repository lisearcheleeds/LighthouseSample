using System.IO;
using Lighthouse.Editor.ScriptableObject;
using UnityEditor;
using UnityEngine;

namespace Lighthouse.Editor.Menu
{
    public class LighthouseEditor : UnityEditor.Editor
    {
        const string SettingsDefaultDirectory = "Assets/Settings";
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
            var settings = (T)CreateInstance(typeof(T));
            if (settings == null)
            {
                return null;
            }

            var fsDirPath = Path.Combine(Application.dataPath, "..", SettingsDefaultDirectory);
            if (!Directory.Exists(fsDirPath))
            {
                Directory.CreateDirectory(fsDirPath);
                AssetDatabase.Refresh();
            }

            var assetPath = $"{SettingsDefaultDirectory}/{typeof(T).Name}{SettingsFileExtension}";
            AssetDatabase.CreateAsset(settings, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return settings;
        }

        static T LoadSettings<T>() where T : UnityEngine.ScriptableObject
        {
            var guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}");
            if (guids.Length == 0)
            {
                return null;
            }

            if (guids.Length > 1)
            {
                Debug.LogWarning($"[LighthouseEditor] Multiple {typeof(T).Name} found. Using the first one.");
            }

            var path = AssetDatabase.GUIDToAssetPath(guids[0]);
            return (T)AssetDatabase.LoadAssetAtPath(path, typeof(T));
        }
    }
}