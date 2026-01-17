using System.Threading;
using Cysharp.Threading.Tasks;

namespace Lighthouse.Core.Scene.SceneTransitionStep
{
    public sealed class OutAnimationStep : ISceneTransitionStep
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
                commonSceneManager.PlayOutAnimation(transitionData.RequireCommonSceneIds, transitionType),
                beforeMainSceneGroup?.PlayOutAnimation(transitionData, transitionType) ?? UniTask.CompletedTask);
        }
    }
}