using System.Threading;
using Cysharp.Threading.Tasks;

namespace Lighthouse.Core.Scene.SceneTransitionStep
{
    public sealed class LeaveSceneStep : ISceneTransitionStep
    {
        async UniTask ISceneTransitionStep.Run(
            TransitionDataBase transitionData,
            TransitionType transitionType,
            MainSceneKey beforeMainSceneKey,
            MainSceneGroup beforeMainSceneGroup,
            MainSceneGroup afterMainSceneGroup,
            ISceneCameraManager sceneCameraManager,
            CommonSceneManager commonSceneManager,
            CancellationToken cancelToken)
        {
            await UniTask.WhenAll(
                beforeMainSceneGroup?.Leave(transitionData, transitionType, cancelToken) ?? UniTask.CompletedTask,
                commonSceneManager.Leave(transitionData, transitionType, cancelToken));
        }
    }
}