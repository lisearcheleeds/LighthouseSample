using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Core.Scene.SceneCamera;

namespace Lighthouse.Core.Scene.SceneTransitionStep
{
    public interface ISceneTransitionStep
    {
        UniTask Run(
            TransitionDataBase transitionData,
            TransitionType transitionType,
            MainSceneKey beforeMainSceneKey,
            MainSceneGroup beforeMainSceneGroup,
            MainSceneGroup afterMainSceneGroup,
            ISceneCameraManager sceneCameraManager,
            ICommonSceneManager commonSceneManager,
            CancellationToken cancelToken);
    }
}