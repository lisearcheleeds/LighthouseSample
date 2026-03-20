using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Lighthouse.Scene.SceneBase
{
    public abstract class ModuleSceneBase : SceneBase
    {
        public abstract ModuleSceneId ModuleSceneId { get; }

        protected override UniTask OnEnter(SceneTransitionContext context, CancellationToken cancelToken)
        {
            if (context.SceneTransitionDiff.ActivateSceneModuleIds.Contains(ModuleSceneId))
            {
                gameObject.SetActive(true);
            }

            return base.OnEnter(context, cancelToken);
        }

        protected override UniTask OnLeave(SceneTransitionContext context, CancellationToken cancelToken)
        {
            if (context.SceneTransitionDiff.DeactivateSceneModuleIds.Contains(ModuleSceneId))
            {
                gameObject.SetActive(false);
            }

            return base.OnLeave(context, cancelToken);
        }
    }
}