using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Scene.SceneCamera;

namespace Lighthouse.Scene.SceneTransitionStep
{
    public sealed class UnloadSceneGroupStep : ISceneTransitionStep
    {
        async UniTask ISceneTransitionStep.Run(
            SceneTransitionContext context,
            CancellationToken cancelToken)
        {
            await context.MainSceneManager.Unload(context.SceneTransitionDiff);
            await context.ModuleSceneManager.Unload(context.SceneTransitionDiff);
        }
    }
}