using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Scene.SceneCamera;

namespace Lighthouse.Scene
{
    public interface IModuleSceneManager
    {
        void SetEnqueueParentLifetimeScope(Func<IDisposable> enqueueParentLifetimeScope);

        public UniTask Load(SceneTransitionContext context);
        public UniTask Unload(SceneTransitionContext context);

        public UniTask Enter(SceneTransitionContext context, CancellationToken cancellationToken);
        public UniTask Leave(SceneTransitionContext context, CancellationToken cancellationToken);

        public void ResetAnimation(SceneTransitionContext context);
        public UniTask PlayInAnimation(SceneTransitionContext context);
        public UniTask PlayOutAnimation(SceneTransitionContext context);

        public ISceneCamera[] GetSceneCameraList(ModuleSceneId[] requestSceneModuleIds);
        public void InitializeCanvas(SceneTransitionContext context);

        public void OnSceneTransitionFinished(SceneTransitionContext context);

        UniTask PreReboot();
    }
}