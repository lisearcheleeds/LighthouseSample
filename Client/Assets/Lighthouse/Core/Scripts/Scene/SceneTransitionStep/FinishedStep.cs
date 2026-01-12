using System.Threading;
using Cysharp.Threading.Tasks;

namespace Lighthouse.Core.Scene.SceneTransitionStep
{
    public sealed class FinishedStep : ISceneTransitionStep
    {
        UniTask ISceneTransitionStep.Run(
            TransitionDataBase transitionData,
            TransitionType transitionType,
            MainSceneKey beforeMainSceneKey,
            MainSceneGroup beforeMainSceneGroup,
            MainSceneGroup afterMainSceneGroup,
            ISceneCameraManager sceneCameraManager,
            CommonSceneManager commonSceneManager,
            CancellationToken cancelToken)
        {
            afterMainSceneGroup.CurrentScene.OnSceneTransitionFinished();
            commonSceneManager.OnSceneTransitionFinished(transitionData.RequireCommonSceneIds);
            return UniTask.CompletedTask;
        }
    }
}