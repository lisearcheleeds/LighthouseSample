using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Scene.SceneCamera;

namespace Lighthouse.Scene.SceneTransitionStep
{
    public sealed class OutAnimationStep : ISceneTransitionStep
    {
        async UniTask ISceneTransitionStep.Run(
            SceneTransitionContext context,
            CancellationToken cancelToken)
        {
            await UniTask.WhenAll(
                context.MainSceneManager.PlayOutAnimation(context.TransitionData, context.TransitionType, context.SceneTransitionDiff),
                context.ModuleSceneManager.PlayOutAnimation(context.TransitionType, context.SceneTransitionDiff));
        }
    }
}