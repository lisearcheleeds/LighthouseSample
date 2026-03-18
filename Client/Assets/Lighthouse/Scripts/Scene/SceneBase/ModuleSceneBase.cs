using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Lighthouse.Scene.SceneBase
{
    public abstract class ModuleSceneBase : SceneBase
    {
        public abstract ModuleSceneId ModuleSceneId { get; }

        public virtual bool IsAlwaysInAnimation { get; protected set; } = false;
        public virtual bool IsAlwaysOutAnimation { get; protected set; } = false;

        protected override UniTask OnEnter(TransitionDataBase transitionData, TransitionType transitionType, SceneTransitionDiff sceneTransitionDiff, CancellationToken cancelToken)
        {
            if (sceneTransitionDiff.ActivateSceneModuleIds.Contains(ModuleSceneId))
            {
                gameObject.SetActive(true);
            }

            return base.OnEnter(transitionData, transitionType, sceneTransitionDiff, cancelToken);
        }

        protected override UniTask OnLeave(TransitionDataBase transitionData, TransitionType transitionType, SceneTransitionDiff sceneTransitionDiff, CancellationToken cancelToken)
        {
            if (sceneTransitionDiff.DeactivateSceneModuleIds.Contains(ModuleSceneId))
            {
                gameObject.SetActive(false);
            }

            return base.OnLeave(transitionData, transitionType, sceneTransitionDiff, cancelToken);
        }
    }
}