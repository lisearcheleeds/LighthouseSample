namespace Lighthouse.Scene
{
    public interface ISceneGroupProvider
    {
        SceneGroup GetSceneGroup(MainSceneId mainSceneId);
    }
}