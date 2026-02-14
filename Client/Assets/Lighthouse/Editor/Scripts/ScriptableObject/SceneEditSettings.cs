using UnityEngine;

namespace Lighthouse.Editor.ScriptableObject
{
    public class SceneEditSettings : UnityEngine.ScriptableObject
    {
        [SerializeField] bool enableSceneEditProcess = true;

        [SerializeField] GameObject canvasSceneEditorOnlyObject;

        [SerializeField] string editorOnlyObjectName = "__EditorOnly__";

        public bool EnableSceneEditProcess => enableSceneEditProcess;

        public GameObject CanvasSceneEditorOnlyObject => canvasSceneEditorOnlyObject;

        public string EditorOnlyObjectName => editorOnlyObjectName;
    }
}