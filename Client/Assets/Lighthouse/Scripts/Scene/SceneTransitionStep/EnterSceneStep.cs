using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Scene.SceneCamera;

namespace Lighthouse.Scene.SceneTransitionStep
{
    public sealed class EnterSceneStep : ISceneTransitionStep
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
            await mainSceneManager.Enter(transitionData, transitionType, sceneTransitionDiff, cancelToken);
            mainSceneManager.ResetInAnimation(transitionData, transitionType, sceneTransitionDiff);

            await moduleSceneManager.Enter(transitionData, transitionType, sceneTransitionDiff, cancelToken);
            moduleSceneManager.ResetAnimation(transitionType, sceneTransitionDiff);
        }
    }
}