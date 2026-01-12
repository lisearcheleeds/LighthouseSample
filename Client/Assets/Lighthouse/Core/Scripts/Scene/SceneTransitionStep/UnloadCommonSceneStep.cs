using System.Threading;
using Cysharp.Threading.Tasks;

namespace Lighthouse.Core.Scene.SceneTransitionStep
{
    public sealed class UnloadCommonSceneStep : ISceneTransitionStep
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
            await commonSceneManager.UnloadUnusedCommonScenes(transitionData.RequireCommonSceneIds);
        }
    }
}