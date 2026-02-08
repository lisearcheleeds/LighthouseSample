using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Core.Scene.SceneCamera;

namespace Lighthouse.Core.Scene.SceneTransitionStep
{
    public sealed class ResolveCameraStep : ISceneTransitionStep
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
            sceneCameraManager.UpdateCameraStack(mainSceneManager, sceneTransitionDiff);

            mainSceneManager.InitializeCanvas(sceneCameraManager, sceneTransitionDiff);
            sceneModuleManager.InitializeCanvas(sceneCameraManager, sceneTransitionDiff);
            
            return UniTask.CompletedTask;
        }
    }
}