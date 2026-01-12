using Lighthouse.Core.Scene.SceneBase;

namespace Lighthouse.Core.EditorTool.SceneEditor
{
    public interface IEditorOnlyObjectCanvasScene
    {
#if UNITY_EDITOR
        void Apply(ICanvasSceneBase[] canvasSceneBaseList);
        void Revoke();
#endif
    }
}