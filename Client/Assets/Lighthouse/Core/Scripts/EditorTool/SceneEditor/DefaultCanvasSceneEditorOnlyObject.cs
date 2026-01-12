using Lighthouse.Core.Scene.SceneBase;
using Product.Util;
using UnityEngine;

namespace Lighthouse.Core.EditorTool.SceneEditor
{
    public class DefaultCanvasSceneEditorOnlyObject : MonoBehaviour, IEditorOnlyObjectCanvasScene
    {
        [SerializeField] CanvasSceneObject canvasSceneObject;

#if UNITY_EDITOR
        public void Apply(ICanvasSceneBase[] canvasSceneBaseList)
        {
            foreach (var canvasSceneBase in canvasSceneBaseList)
            {
                canvasSceneBase.InitializeCanvas(canvasSceneObject.UICamera);
            }
        }

        public void Revoke()
        {
            DestroyImmediate(this);
        }
#endif
    }
}