using System.Threading;
using Cysharp.Threading.Tasks;

namespace Lighthouse.Scene.SceneTransitionStep
{
    public sealed class LoadNextSceneStateStep : ISceneTransitionStep
    {
        async UniTask ISceneTransitionStep.Run(
            SceneTransitionContext context,
            CancellationToken cancelToken)
        {
            await context.TransitionData.LoadSceneState(context.TransitionDirectionType, cancelToken);
        }
    }
}