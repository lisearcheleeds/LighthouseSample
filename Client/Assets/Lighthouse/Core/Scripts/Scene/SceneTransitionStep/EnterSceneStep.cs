using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Core.Scene.SceneCamera;

namespace Lighthouse.Core.Scene.SceneTransitionStep
{
    public sealed class EnterSceneStep : ISceneTransitionStep
    {
        async UniTask ISceneTransitionStep.Run(
            TransitionDataBase transitionData,
            TransitionType transitionType,
            MainSceneKey beforeMainSceneKey,
            MainSceneGroup beforeMainSceneGroup,
            MainSceneGroup afterMainSceneGroup,
            ISceneCameraManager sceneCameraManager,
            ICommonSceneManager commonSceneManager,
            CancellationToken cancelToken)
        {
            await UniTask.WhenAll(
                afterMainSceneGroup?.Enter(transitionData, transitionType, cancelToken) ?? UniTask.CompletedTask,
                commonSceneManager.Enter(transitionData, transitionType, cancelToken));
        }
    }
}