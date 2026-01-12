using Lighthouse.Core.Scene;
using Product.Util;
using UnityEngine;

namespace Lighthouse.Editor.PostProcess.SceneEditor
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