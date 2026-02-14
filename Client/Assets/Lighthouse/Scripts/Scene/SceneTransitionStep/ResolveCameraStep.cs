using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Scene.SceneCamera;

namespace Lighthouse.Scene.SceneTransitionStep
{
    public sealed class ResolveCameraStep : ISceneTransitionStep
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
            sceneCameraManager.UpdateCameraStack(mainSceneManager, sceneTransitionDiff);

            mainSceneManager.InitializeCanvas(sceneCameraManager, sceneTransitionDiff);
            moduleSceneManager.InitializeCanvas(sceneCameraManager, sceneTransitionDiff);
            
            return UniTask.CompletedTask;
        }
    }
}