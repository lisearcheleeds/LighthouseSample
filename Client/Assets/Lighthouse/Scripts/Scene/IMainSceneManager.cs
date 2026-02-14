using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Scene.SceneCamera;

namespace Lighthouse.Scene
{
    public interface IMainSceneManager
    {
        MainSceneId CurrentMainSceneId { get; }

        void SetEnqueueParentLifetimeScope(Func<IDisposable> enqueueParentLifetimeScope);

        UniTask Load(SceneTransitionDiff sceneTransitionDiff);
        UniTask Unload(SceneTransitionDiff sceneTransitionDiff);

        UniTask Enter(TransitionDataBase transitionData, TransitionType transitionType, SceneTransitionDiff sceneTransitionDiff, CancellationToken cancellationToken);
        UniTask Leave(TransitionDataBase transitionData, TransitionType transitionType, SceneTransitionDiff sceneTransitionDiff, CancellationToken cancellationToken);

        void ResetInAnimation(TransitionDataBase transitionData, TransitionType transitionType, SceneTransitionDiff sceneTransitionDiff);
        UniTask PlayInAnimation(TransitionDataBase transitionData, TransitionType transitionType, SceneTransitionDiff sceneTransitionDiff);
        UniTask PlayOutAnimation(TransitionDataBase transitionData, TransitionType transitionType, SceneTransitionDiff sceneTransitionDiff);

        UniTask SaveSceneState(CancellationToken cancelToken);

        ISceneCamera[] GetSceneCameraList(SceneTransitionDiff sceneTransitionDiff);
        void InitializeCanvas(ISceneCameraManager sceneGroupManager, SceneTransitionDiff sceneTransitionDiff);

        void Suspend();
        void Resume();

        void OnSceneTransitionFinished(SceneTransitionDiff sceneTransitionDiff);
    }
}