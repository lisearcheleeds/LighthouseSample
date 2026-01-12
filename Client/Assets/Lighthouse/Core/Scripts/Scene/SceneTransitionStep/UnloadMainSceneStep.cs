using System.Threading;
using Cysharp.Threading.Tasks;

namespace Lighthouse.Core.Scene
{
    public sealed class UnloadMainSceneStep : ISceneTransitionStep
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
            if (beforeMainSceneGroup != null && !ReferenceEquals(beforeMainSceneGroup, afterMainSceneGroup))
            {
                await beforeMainSceneGroup.Unload();
            }
        }
    }
}