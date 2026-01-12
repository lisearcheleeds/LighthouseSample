using System.Linq;
using Cysharp.Threading.Tasks;
using Lighthouse.Core.Scene;
using Lighthouse.Editor.ScriptableObject;
using Lighthouse.Editor.Scripts;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

namespace Lighthouse.Editor.PostProcess.SceneEditor
{
    [InitializeOnLoad]
    public static class SceneEditProcessor
    {
        static readonly SceneEditSettings sceneEditSettings;

        static SceneEditProcessor()
        {
            if (Application.isBatchMode || BuildPipeline.isBuildingPlayer)
            {
                return;
            }

            sceneEditSettings = LighthouseEditor.GetSettings<SceneEditSettings>();
            if (sceneEditSettings.EnableSceneEditProcess)
            {
                SetupSceneEditProcessor();
            }
        }

        static void SetupSceneEditProcessor()
        {
            EditorSceneManager.sceneOpened += (scene, mode) =>
            {
                DestroyEditorOnlyObject();
                InitEditorOnlyObject();
            };

            EditorSceneManager.sceneClosed += scene =>
            {
                DestroyEditorOnlyObject();
            };

            EditorApplication.playModeStateChanged += mode =>
            {
                switch (mode)
                {
                    case PlayModeStateChange.EnteredEditMode:
                        DestroyEditorOnlyObject();
                        InitEditorOnlyObject();
                        break;
                    case PlayModeStateChange.ExitingPlayMode:
                        DestroyEditorOnlyObject();
                        break;
                }
            };

            if (!EditorApplication.isPlayingOrWillChangePlaymode)
            {
                EditorApplication.delayCall += () =>
                {
                    DestroyEditorOnlyObject();
                    InitEditorOnlyObject();
                };
            }
        }

        static void InitEditorOnlyObject()
        {
            if (!sceneEditSettings.EnableSceneEditProcess || sceneEditSettings.CanvasSceneEditorOnlyObject == null)
            {
                return;
            }

            var canvasSceneBaseList = GetSceneRootComponentList<ICanvasSceneBase>();
            if (!canvasSceneBaseList.Any())
            {
                return;
            }

            var editorOnlyObject = Object.Instantiate(sceneEditSettings.CanvasSceneEditorOnlyObject);
            editorOnlyObject.name = sceneEditSettings.EditorOnlyObjectName;

            SetHideFlags(editorOnlyObject.transform, HideFlags.DontSaveInEditor);

            var editorOnlyComponents = editorOnlyObject.GetComponents<MonoBehaviour>().OfType<IEditorOnlyObjectCanvasScene>().ToArray();
            foreach (var editorOnlyComponent in editorOnlyComponents)
            {
                editorOnlyComponent.Apply(canvasSceneBaseList);
            }
        }

        static void DestroyEditorOnlyObject()
        {
            var editorOnlyObjectList = GetSceneRootComponentList<IEditorOnlyObjectCanvasScene>();
            foreach (var editorOnlyObject in editorOnlyObjectList)
            {
                editorOnlyObject.Revoke();
            }
        }

        static T[] GetSceneRootComponentList<T>()
        {
            return Enumerable.Range(0, SceneManager.sceneCount)
                .Select(SceneManager.GetSceneAt)
                .Where(scene => scene.IsValid() && scene.isLoaded)
                .SelectMany(scene => scene.GetRootGameObjects()
                    .SelectMany(x => x.GetComponents<MonoBehaviour>().OfType<T>()))
                .ToArray();
        }

        static void SetHideFlags(Transform obj, HideFlags flags)
        {
            obj.gameObject.hideFlags = flags;
            foreach (var child in obj.Cast<Transform>())
            {
                SetHideFlags(child, flags);
            }
        }
    }
}