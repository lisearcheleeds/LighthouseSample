using System.Threading;
using Cysharp.Threading.Tasks;

namespace Lighthouse.Scene.SceneTransitionStep
{
    public sealed class LoadSceneGroupStep : ISceneTransitionStep
    {
        async UniTask ISceneTransitionStep.Run(
            SceneTransitionContext context,
            CancellationToken cancelToken)
        {
            await context.MainSceneManager.Load(context);
            context.MainSceneManager.ResetInAnimation(context);

            await context.ModuleSceneManager.Load(context);
            context.ModuleSceneManager.ResetAnimation(context);
        }
    }
}