using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Scene.SceneCamera;

namespace Lighthouse.Scene.SceneTransitionStep
{
    public sealed class LoadSceneGroupStep : ISceneTransitionStep
    {
        async UniTask ISceneTransitionStep.Run(
            TransitionDataBase transitionData,
            TransitionType transitionType,
            SceneTransitionDiff sceneTransitionDiff,
            IMainSceneManager mainSceneManager,
            IModuleSceneManager moduleSceneManager,
            ISceneCameraManager sceneCameraManager,
            CancellationToken cancelToken)
        {
            await mainSceneManager.Load(sceneTransitionDiff);
            mainSceneManager.ResetInAnimation(transitionData, transitionType, sceneTransitionDiff);

            await moduleSceneManager.Load(sceneTransitionDiff);
            moduleSceneManager.ResetAnimation(transitionType, sceneTransitionDiff);
        }
    }
}