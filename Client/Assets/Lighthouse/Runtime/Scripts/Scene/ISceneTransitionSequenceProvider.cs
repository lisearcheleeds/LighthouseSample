using Lighthouse.Scene.SceneTransitionPhase;

namespace Lighthouse.Scene
{
    public interface ISceneTransitionSequenceProvider
    {
        ISceneTransitionPhase[] ExclusiveSequence { get; }
        ISceneTransitionPhase[] CrossSequence { get; }
    }
}