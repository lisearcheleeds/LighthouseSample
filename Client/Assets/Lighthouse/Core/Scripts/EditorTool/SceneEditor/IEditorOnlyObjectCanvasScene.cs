using Lighthouse.Core.Scene;

namespace Lighthouse.Editor.PostProcess.SceneEditor
{
    public interface IEditorOnlyObjectCanvasScene
    {
#if UNITY_EDITOR
        void Apply(ICanvasSceneBase[] canvasSceneBaseList);
        void Revoke();
#endif
    }
}