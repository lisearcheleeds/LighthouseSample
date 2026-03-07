using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Scene.SceneCamera;

namespace Lighthouse.Scene.SceneTransitionStep
{
    public sealed class ResolveCameraStep : ISceneTransitionStep
    {
        UniTask ISceneTransitionStep.Run(
            SceneTransitionContext context,
            CancellationToken cancelToken)
        {
            context.SceneCameraManager.UpdateCameraStack(context.MainSceneManager, context.SceneTransitionDiff);

            context.MainSceneManager.InitializeCanvas(context.SceneCameraManager, context.SceneTransitionDiff);
            context.ModuleSceneManager.InitializeCanvas(context.SceneCameraManager, context.SceneTransitionDiff);

            return UniTask.CompletedTask;
        }
    }
}