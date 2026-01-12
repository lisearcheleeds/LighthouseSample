using System.Threading;
using Cysharp.Threading.Tasks;

namespace Lighthouse.Core.Scene.SceneTransitionStep
{
    public sealed class InAnimationStep : ISceneTransitionStep
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
                commonSceneManager.InAnimation(transitionData.RequireCommonSceneIds, transitionType),
                afterMainSceneGroup.InAnimation(transitionData, transitionType));
        }
    }
}