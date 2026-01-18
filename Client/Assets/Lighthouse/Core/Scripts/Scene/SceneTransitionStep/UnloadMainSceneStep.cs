using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Core.Scene.SceneCamera;

namespace Lighthouse.Core.Scene.SceneTransitionStep
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
            ICommonSceneManager commonSceneManager,
            CancellationToken cancelToken)
        {
            if (beforeMainSceneGroup != null && !ReferenceEquals(beforeMainSceneGroup, afterMainSceneGroup))
            {
                await beforeMainSceneGroup.Unload();
            }
        }
    }
}