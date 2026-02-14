using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Scene.SceneCamera;

namespace Lighthouse.Scene
{
    public interface IModuleSceneManager
    {
        void SetEnqueueParentLifetimeScope(Func<IDisposable> enqueueParentLifetimeScope);

        public UniTask Load(SceneTransitionDiff sceneTransitionDiff);
        public UniTask Unload(SceneTransitionDiff sceneTransitionDiff);

        public UniTask Enter(TransitionDataBase transitionData, TransitionType transitionType, SceneTransitionDiff sceneTransitionDiff, CancellationToken cancellationToken);
        public UniTask Leave(TransitionDataBase transitionData, TransitionType transitionType, SceneTransitionDiff sceneTransitionDiff, CancellationToken cancellationToken);

        public void ResetAnimation(TransitionType transitionType, SceneTransitionDiff sceneTransitionDiff);
        public UniTask PlayInAnimation(TransitionType transitionType, SceneTransitionDiff sceneTransitionDiff);
        public UniTask PlayOutAnimation(TransitionType transitionType, SceneTransitionDiff sceneTransitionDiff);

        public ISceneCamera[] GetSceneCameraList(ModuleSceneId[] requestSceneModuleIds);
        public void InitializeCanvas(ISceneCameraManager sceneGroupManager, SceneTransitionDiff sceneTransitionDiff);

        public void OnSceneTransitionFinished(SceneTransitionDiff sceneTransitionDiff);
    }
}