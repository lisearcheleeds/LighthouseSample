using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Scene.SceneCamera;

namespace Lighthouse.Scene
{
    public interface IMainSceneManager
    {
        void SetEnqueueParentLifetimeScope(Func<IDisposable> enqueueParentLifetimeScope);

        UniTask Load(SceneTransitionContext context);
        UniTask Unload(SceneTransitionContext context);

        UniTask Enter(SceneTransitionContext context, CancellationToken cancelToken);
        UniTask Leave(SceneTransitionContext context, CancellationToken cancelToken);

        void ResetInAnimation(SceneTransitionContext context);
        UniTask PlayInAnimation(SceneTransitionContext context);
        UniTask PlayOutAnimation(SceneTransitionContext context);

        UniTask SaveSceneState(SceneTransitionContext context, CancellationToken cancelToken);

        ISceneCamera[] GetSceneCameraList(SceneTransitionDiff sceneTransitionDiff);
        void InitializeCanvas(SceneTransitionContext context);

        void OnSceneTransitionFinished(SceneTransitionContext context);

        UniTask PreReboot();
    }
}