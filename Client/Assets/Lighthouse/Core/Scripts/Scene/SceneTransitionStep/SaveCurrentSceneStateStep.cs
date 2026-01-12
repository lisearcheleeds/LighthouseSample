using System.Threading;
using Cysharp.Threading.Tasks;

namespace Lighthouse.Core.Scene.SceneTransitionStep
{
    public sealed class SaveCurrentSceneStateStep : ISceneTransitionStep
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
            if (beforeMainSceneGroup != null)
            {
                await beforeMainSceneGroup.SaveSceneState(cancelToken);
            }
        }
    }
}