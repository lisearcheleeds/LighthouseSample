namespace Lighthouse.Core.Scene
{
    public interface ISceneGroupProvider
    {
        SceneGroup GetSceneGroup(MainSceneId mainSceneId);
    }
}