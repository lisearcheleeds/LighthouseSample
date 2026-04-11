using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LighthouseExtends.TextTable.Editor
{
    public class MissingKeysWindow : EditorWindow
    {
        List<(string scope, string hierarchyPath, string textKey)> missingRefs;
        Vector2 scroll;

        void OnGUI()
        {
            EditorGUILayout.LabelField($"{missingRefs.Count} missing TextKey reference(s) found:", EditorStyles.boldLabel);
            EditorGUILayout.Space(4);

            scroll = EditorGUILayout.BeginScrollView(scroll);

            foreach (var (scope, path, key) in missingRefs)
            {
                using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                {
                    EditorGUILayout.LabelField($"Scope: {scope}", EditorStyles.boldLabel);
                    EditorGUILayout.LabelField($"Path:  {path}", EditorStyles.miniLabel);
                    EditorGUILayout.LabelField($"Key:   {key}", EditorStyles.miniLabel);
                }

                EditorGUILayout.Space(2);
            }

            EditorGUILayout.EndScrollView();
        }

        public static void Show(List<(string, string, string)> missingRefs)
        {
            var window = GetWindow<MissingKeysWindow>(true, "Missing TextKey References", true);
            window.missingRefs = missingRefs;
            window.minSize = new Vector2(520, 300);
        }
    }
}
