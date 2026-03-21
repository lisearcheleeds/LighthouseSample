using Cysharp.Threading.Tasks;
using Lighthouse.Scene;

namespace LighthouseExtends.Popup
{
    public interface IPopupManager
    {
        void Setup();

        UniTask EnqueuePopup(IPopupData popupData);
        UniTask OpenPopup();
        UniTask OpenPopup(IPopupData popupData);

        UniTask ClosePopup();
        UniTask ClosePopup(IPopupData popupData);

        UniTask ClearAllPopup();
        UniTask ClearCurrentAllPopup();

        UniTask ResumePopupFromSceneId(MainSceneId mainSceneId, bool isPlayInAnimation);
        UniTask SuspendPopupFromSceneId(MainSceneId mainSceneId);
    }
}
