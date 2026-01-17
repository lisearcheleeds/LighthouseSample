using System.Threading;
using Cysharp.Threading.Tasks;

namespace Lighthouse.Core.Scene.SceneTransitionStep
{
    public sealed class LoadMainSceneStep : ISceneTransitionStep
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
            if (!ReferenceEquals(beforeMainSceneGroup, afterMainSceneGroup))
            {
                await afterMainSceneGroup.Load(transitionData);
            }

            await afterMainSceneGroup.PlayResetAnimation(transitionType);
        }
    }
}