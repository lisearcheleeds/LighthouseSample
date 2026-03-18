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

        protected override async UniTask OnEnter(TransitionDataBase transitionData, TransitionType transitionType, SceneTransitionDiff sceneTransitionDiff, CancellationToken cancelToken)
        {
            if (transitionType == TransitionType.Back)
            {
                await popupManager.ResumePopupFromSceneId(sceneTransitionDiff.NextMainSceneId, cancelToken);
            }

            await base.OnEnter(transitionData, transitionType, sceneTransitionDiff, cancelToken);
        }

        protected override async UniTask OnLeave(TransitionDataBase transitionData, TransitionType transitionType, SceneTransitionDiff sceneTransitionDiff, CancellationToken cancelToken)
        {
            if (transitionType == TransitionType.Default)
            {
                await popupManager.SuspendPopupFromSceneId(sceneTransitionDiff.CurrentMainSceneId, cancelToken);
            }

            await base.OnLeave(transitionData, transitionType, sceneTransitionDiff, cancelToken);
        }
    }
}