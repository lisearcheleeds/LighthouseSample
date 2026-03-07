using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Scene.SceneCamera;

namespace Lighthouse.Scene.SceneTransitionStep
{
    public sealed class LoadSceneGroupStep : ISceneTransitionStep
    {
        async UniTask ISceneTransitionStep.Run(
            SceneTransitionContext context,
            CancellationToken cancelToken)
        {
            await context.MainSceneManager.Load(context.SceneTransitionDiff);
            context.MainSceneManager.ResetInAnimation(context.TransitionData, context.TransitionType, context.SceneTransitionDiff);

            await context.ModuleSceneManager.Load(context.SceneTransitionDiff);
            context.ModuleSceneManager.ResetAnimation(context.TransitionType, context.SceneTransitionDiff);
        }
    }
}