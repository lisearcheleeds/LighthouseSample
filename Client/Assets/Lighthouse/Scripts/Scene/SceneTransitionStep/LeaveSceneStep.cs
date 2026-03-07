using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Scene.SceneCamera;

namespace Lighthouse.Scene.SceneTransitionStep
{
    public sealed class LeaveSceneStep : ISceneTransitionStep
    {
        async UniTask ISceneTransitionStep.Run(
            SceneTransitionContext context,
            CancellationToken cancelToken)
        {
            await context.MainSceneManager.Leave(context.TransitionData, context.TransitionType, context.SceneTransitionDiff, cancelToken);
            await context.ModuleSceneManager.Leave(context.TransitionData, context.TransitionType, context.SceneTransitionDiff, cancelToken);
        }
    }
}