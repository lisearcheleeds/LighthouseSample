using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Core.Scene.SceneCamera;

namespace Lighthouse.Core.Scene.SceneTransitionStep
{
    public sealed class InAnimationStep : ISceneTransitionStep
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
            await UniTask.WhenAll(
                mainSceneManager.PlayInAnimation(transitionData, transitionType, sceneTransitionDiff),
                sceneModuleManager.PlayInAnimation(transitionType, sceneTransitionDiff));
        }
    }
}