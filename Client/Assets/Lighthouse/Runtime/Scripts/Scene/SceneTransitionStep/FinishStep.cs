using System.Threading;
using Cysharp.Threading.Tasks;

namespace Lighthouse.Scene.SceneTransitionStep
{
    public sealed class FinishStep : ISceneTransitionStep
    {
        UniTask ISceneTransitionStep.Run(
            ISceneTransitionContext context,
            CancellationToken cancelToken)
        {
            context.MainSceneManager.OnSceneTransitionFinished(context);
            context.ModuleSceneManager.OnSceneTransitionFinished(context);
            return UniTask.CompletedTask;
        }
    }
}
