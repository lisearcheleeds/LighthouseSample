using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Core.Scene.SceneCamera;

namespace Lighthouse.Core.Scene.SceneTransitionStep
{
    public sealed class FinishStep : ISceneTransitionStep
    {
        UniTask ISceneTransitionStep.Run(
            TransitionDataBase transitionData,
            TransitionType transitionType,
            SceneTransitionDiff sceneTransitionDiff,
            IMainSceneManager mainSceneManager,
            ISceneModuleManager sceneModuleManager,
            ISceneCameraManager sceneCameraManager,
            CancellationToken cancelToken)
        {
            mainSceneManager.OnSceneTransitionFinished(sceneTransitionDiff);
            sceneModuleManager.OnSceneTransitionFinished(sceneTransitionDiff);
            return UniTask.CompletedTask;
        }
    }
}