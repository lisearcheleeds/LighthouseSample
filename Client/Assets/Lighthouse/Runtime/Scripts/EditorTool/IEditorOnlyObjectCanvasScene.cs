#if UNITY_EDITOR
using Lighthouse.Scene.SceneBase;

namespace Lighthouse.EditorTool
{
    public interface IEditorOnlyObjectCanvasScene
    {
        void Apply(ICanvasSceneBase[] canvasSceneBaseList);
        void Revoke();
    }
}
#endif