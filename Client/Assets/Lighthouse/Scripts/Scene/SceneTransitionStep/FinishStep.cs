using System.Threading;
using Cysharp.Threading.Tasks;

namespace Lighthouse.Scene.SceneTransitionStep
{
    public sealed class FinishStep : ISceneTransitionStep
    {
        UniTask ISceneTransitionStep.Run(
            SceneTransitionContext context,
            CancellationToken cancelToken)
        {
            context.MainSceneManager.OnSceneTransitionFinished(context.SceneTransitionDiff);
            context.ModuleSceneManager.OnSceneTransitionFinished(context.SceneTransitionDiff);
            return UniTask.CompletedTask;
        }
    }
}