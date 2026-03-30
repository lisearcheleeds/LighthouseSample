namespace LighthouseExtends.Animation.Runtime
{
    public interface ILHSceneTransitionAnimator : ILHTransitionAnimator
    {
        bool InvokeResetInAnimation { get; }
        bool InvokeInAnimation { get; }
        bool InvokeOutAnimation { get; }
    }
}