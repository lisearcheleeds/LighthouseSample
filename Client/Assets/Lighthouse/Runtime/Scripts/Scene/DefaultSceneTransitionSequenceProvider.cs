using Lighthouse.Scene.SceneTransitionPhase;

namespace Lighthouse.Scene
{
    public sealed class DefaultSceneTransitionSequenceProvider : ISceneTransitionSequenceProvider
    {
        ISceneTransitionPhase[] ISceneTransitionSequenceProvider.CrossSequence { get; } =
        {
            new StartTransitionPhase(),
            new LoadSceneGroupPhase(),
            new EnterScenePhase(),
            new CrossAnimationPhase(),
            new LeaveScenePhase(),
            new UnloadSceneGroupPhase(),
            new FinishTransitionPhase(),
            new CleanupPhase(),
        };

        ISceneTransitionPhase[] ISceneTransitionSequenceProvider.ExclusiveSequence { get; } =
        {
            new StartTransitionPhase(),
            new OutAnimationPhase(),
            new LeaveScenePhase(),
            new LoadSceneGroupPhase(),
            new UnloadSceneGroupPhase(),
            new EnterScenePhase(),
            new InAnimationPhase(),
            new FinishTransitionPhase(),
            new CleanupPhase(),
        };
    }
}