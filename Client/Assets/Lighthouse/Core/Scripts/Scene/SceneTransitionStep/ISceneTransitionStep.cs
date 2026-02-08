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
            SceneTransitionDiff sceneTransitionDiff,
            IMainSceneManager mainSceneManager,
            ISceneModuleManager sceneModuleManager,
            ISceneCameraManager sceneCameraManager,
            CancellationToken cancelToken);
    }
}