namespace Lighthouse.Scene.SceneBase
{
    public abstract class ModuleSceneBase : SceneBase
    {
        public abstract ModuleSceneId ModuleSceneId { get; }

        public virtual bool IsAlwaysInAnimation { get; protected set; } = false;
        public virtual bool IsAlwaysOutAnimation { get; protected set; } = false;
    }
}