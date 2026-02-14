using Lighthouse.Scene.SceneBase;
using LighthouseExtends.CanvasSceneObject;
using UnityEngine;

namespace Lighthouse.EditorTool.SceneEditor
{
    public class DefaultCanvasSceneEditorOnlyObject : MonoBehaviour, IEditorOnlyObjectCanvasScene
    {
        [SerializeField] LHCanvasSceneObject lhCanvasSceneObject;

#if UNITY_EDITOR
        public void Apply(ICanvasSceneBase[] canvasSceneBaseList)
        {
            foreach (var canvasSceneBase in canvasSceneBaseList)
            {
                canvasSceneBase.InitializeCanvas(lhCanvasSceneObject.UICamera);
            }
        }

        public void Revoke()
        {
            DestroyImmediate(this);
        }
#endif
    }
}