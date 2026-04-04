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
            await context.MainSceneManager.Enter(context, cancelToken);
            context.MainSceneManager.ResetInAnimation(context);

            await context.ModuleSceneManager.Enter(context, cancelToken);
            context.ModuleSceneManager.ResetAnimation(context);
        }
    }
}