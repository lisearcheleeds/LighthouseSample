using LighthouseExtends.UIComponent.Editor.Button;
using SampleProduct.View.Common;
using UnityEditor;

namespace SampleProduct.Editor.View.Common
{
    [CustomEditor(typeof(OpenURLButton))]
    [CanEditMultipleObjects]
    public sealed class OpenURLButtonEditor : LHButtonEditor
    {
        SerializedProperty urlProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
            urlProperty = serializedObject.FindProperty("url");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            serializedObject.Update();
            EditorGUILayout.PropertyField(urlProperty);
            serializedObject.ApplyModifiedProperties();
        }
    }
}