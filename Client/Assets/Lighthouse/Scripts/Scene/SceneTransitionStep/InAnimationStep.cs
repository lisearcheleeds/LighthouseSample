using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Scene.SceneCamera;

namespace Lighthouse.Scene.SceneTransitionStep
{
    public sealed class InAnimationStep : ISceneTransitionStep
    {
        async UniTask ISceneTransitionStep.Run(
            SceneTransitionContext context,
            CancellationToken cancelToken)
        {
            await UniTask.WhenAll(
                context.MainSceneManager.PlayInAnimation(context.TransitionData, context.TransitionType, context.SceneTransitionDiff),
                context.ModuleSceneManager.PlayInAnimation(context.TransitionType, context.SceneTransitionDiff));
        }
    }
}