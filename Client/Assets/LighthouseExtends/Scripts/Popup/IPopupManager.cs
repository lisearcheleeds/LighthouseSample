using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Scene;

namespace LighthouseExtends.Popup
{
    public interface IPopupManager
    {
        UniTask EnqueuePopup(IPopupData popupData, CancellationToken token);
        UniTask OpenPopup(CancellationToken token);
        UniTask OpenPopup(IPopupData popupData, CancellationToken token);

        UniTask ClosePopup(CancellationToken token);
        UniTask ClosePopup(IPopupData popupData, CancellationToken token);

        UniTask ClearAllPopup(CancellationToken token);
        UniTask ClearCurrentAllPopup(CancellationToken token);

        UniTask ResumePopupFromSceneId(MainSceneId mainSceneId, CancellationToken token);
        UniTask SuspendPopupFromSceneId(MainSceneId mainSceneId, CancellationToken token);
    }
}