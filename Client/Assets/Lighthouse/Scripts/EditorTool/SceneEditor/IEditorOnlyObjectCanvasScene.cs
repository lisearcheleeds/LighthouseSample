using Lighthouse.Scene.SceneBase;

namespace Lighthouse.EditorTool.SceneEditor
{
    public interface IEditorOnlyObjectCanvasScene
    {
#if UNITY_EDITOR
        void Apply(ICanvasSceneBase[] canvasSceneBaseList);
        void Revoke();
#endif
    }
}