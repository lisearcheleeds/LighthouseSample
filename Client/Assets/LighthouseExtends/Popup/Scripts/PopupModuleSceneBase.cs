using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
using Lighthouse.Scene.SceneBase;
using VContainer;

namespace LighthouseExtends.Popup
{
    public abstract class PopupModuleSceneBase : CanvasModuleSceneBase
    {
        IPopupManager popupManager;

        [Inject]
        public void Constructor(IPopupManager popupManager)
        {
            this.popupManager = popupManager;
        }

        protected override UniTask OnSetup()
        {
            popupManager.Setup();
            return UniTask.CompletedTask;
        }

        protected override UniTask OnEnter(SceneTransitionContext context, CancellationToken cancelToken)
        {
            if (context.TransitionDirectionType == TransitionDirectionType.Back && context.TransitionType == TransitionType.Exclusive)
            {
                return popupManager.ResumePopupFromSceneId(context.SceneTransitionDiff.NextMainSceneId, false);
            }

            return UniTask.CompletedTask;
        }

        protected override UniTask OnLeave(SceneTransitionContext context, CancellationToken cancelToken)
        {
            if (context.TransitionDirectionType == TransitionDirectionType.Forward && context.TransitionType == TransitionType.Exclusive)
            {
                return popupManager.SuspendPopupFromSceneId(context.SceneTransitionDiff.CurrentMainSceneId);
            }

            return UniTask.CompletedTask;
        }

        protected override UniTask InAnimation(SceneTransitionContext context)
        {
            if (context.TransitionDirectionType == TransitionDirectionType.Back && context.TransitionType == TransitionType.Cross)
            {
                return popupManager.ResumePopupFromSceneId(context.SceneTransitionDiff.NextMainSceneId, true);
            }

            return UniTask.CompletedTask;
        }

        protected override UniTask OutAnimation(SceneTransitionContext context)
        {
            if (context.TransitionDirectionType == TransitionDirectionType.Forward && context.TransitionType == TransitionType.Cross)
            {
                return popupManager.SuspendPopupFromSceneId(context.SceneTransitionDiff.CurrentMainSceneId);
            }

            return UniTask.CompletedTask;
        }
    }
}