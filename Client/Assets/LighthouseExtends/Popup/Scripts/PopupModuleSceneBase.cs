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

        protected override UniTask OnEnter(TransitionDataBase transitionData, TransitionType transitionType, bool isActivateScene, CancellationToken cancelToken)
        {
            if (transitionType == TransitionType.Back)
            {
                popupManager.ResumePopupFromSceneId(transitionData.MainSceneId, cancelToken);
            }

            return base.OnEnter(transitionData, transitionType, isActivateScene, cancelToken);
        }

        protected override UniTask OnLeave(TransitionDataBase transitionData, TransitionType transitionType, bool isDeactivateScene, CancellationToken cancelToken)
        {
            if (transitionType == TransitionType.Default)
            {
                popupManager.SuspendPopupFromSceneId(transitionData.MainSceneId, cancelToken);
            }

            return base.OnLeave(transitionData, transitionType, isDeactivateScene, cancelToken);
        }
    }
}