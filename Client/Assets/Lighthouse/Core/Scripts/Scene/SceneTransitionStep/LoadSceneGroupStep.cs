using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Core.Scene.SceneCamera;

namespace Lighthouse.Core.Scene.SceneTransitionStep
{
    public sealed class LoadSceneGroupStep : ISceneTransitionStep
    {
        async UniTask ISceneTransitionStep.Run(
            TransitionDataBase transitionData,
            TransitionType transitionType,
            SceneTransitionDiff sceneTransitionDiff,
            IMainSceneManager mainSceneManager,
            ISceneModuleManager sceneModuleManager,
            ISceneCameraManager sceneCameraManager,
            CancellationToken cancelToken)
        {
            await mainSceneManager.Load(sceneTransitionDiff);
            mainSceneManager.ResetInAnimation(transitionData, transitionType, sceneTransitionDiff);

            await sceneModuleManager.Load(sceneTransitionDiff);
            sceneModuleManager.ResetAnimation(transitionType, sceneTransitionDiff);
        }
    }
}