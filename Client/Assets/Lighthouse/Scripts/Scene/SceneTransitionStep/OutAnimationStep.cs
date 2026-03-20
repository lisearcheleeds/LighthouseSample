using System.Threading;
using Cysharp.Threading.Tasks;

namespace Lighthouse.Scene.SceneTransitionStep
{
    public sealed class OutAnimationStep : ISceneTransitionStep
    {
        async UniTask ISceneTransitionStep.Run(
            SceneTransitionContext context,
            CancellationToken cancelToken)
        {
            await UniTask.WhenAll(
                context.MainSceneManager.PlayOutAnimation(context),
                context.ModuleSceneManager.PlayOutAnimation(context));
        }
    }
}