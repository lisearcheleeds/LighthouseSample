using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Scene.SceneCamera;

namespace Lighthouse.Scene.SceneTransitionStep
{
    public sealed class FinishStep : ISceneTransitionStep
    {
        UniTask ISceneTransitionStep.Run(
            TransitionDataBase transitionData,
            TransitionType transitionType,
            SceneTransitionDiff sceneTransitionDiff,
            IMainSceneManager mainSceneManager,
            IModuleSceneManager moduleSceneManager,
            ISceneCameraManager sceneCameraManager,
            CancellationToken cancelToken)
        {
            mainSceneManager.OnSceneTransitionFinished(sceneTransitionDiff);
            moduleSceneManager.OnSceneTransitionFinished(sceneTransitionDiff);
            return UniTask.CompletedTask;
        }
    }
}