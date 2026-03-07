using System.Threading;
using Cysharp.Threading.Tasks;

namespace Lighthouse.Scene.SceneTransitionStep
{
    public sealed class EnterSceneStep : ISceneTransitionStep
    {
        async UniTask ISceneTransitionStep.Run(
            SceneTransitionContext context,
            CancellationToken cancelToken)
        {
            await context.MainSceneManager.Enter(context.TransitionData, context.TransitionType, context.SceneTransitionDiff, cancelToken);
            context.MainSceneManager.ResetInAnimation(context.TransitionData, context.TransitionType, context.SceneTransitionDiff);

            await context.ModuleSceneManager.Enter(context.TransitionData, context.TransitionType, context.SceneTransitionDiff, cancelToken);
            context.ModuleSceneManager.ResetAnimation(context.TransitionType, context.SceneTransitionDiff);
        }
    }
}