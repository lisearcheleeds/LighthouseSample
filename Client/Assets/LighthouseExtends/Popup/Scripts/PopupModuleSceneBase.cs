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

        protected override async UniTask OnEnter(SceneTransitionContext context, CancellationToken cancelToken)
        {
            if (context.TransitionDirectionType == TransitionDirectionType.Back)
            {
                await popupManager.ResumePopupFromSceneId(context.SceneTransitionDiff.NextMainSceneId, cancelToken);
            }

            await base.OnEnter(context, cancelToken);
        }

        protected override async UniTask OnLeave(SceneTransitionContext context, CancellationToken cancelToken)
        {
            if (context.TransitionDirectionType == TransitionDirectionType.Forward)
            {
                await popupManager.SuspendPopupFromSceneId(context.SceneTransitionDiff.CurrentMainSceneId, cancelToken);
            }

            await base.OnLeave(context, cancelToken);
        }
    }
}