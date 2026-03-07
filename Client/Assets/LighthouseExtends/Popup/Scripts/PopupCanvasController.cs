using UnityEngine;

namespace LighthouseExtends.Popup
{
    public class PopupCanvasController : MonoBehaviour, IPopupCanvasController
    {
        [SerializeField] Canvas defaultLayerCanvas;
        [SerializeField] Canvas systemLayerCanvas;

        void IPopupCanvasController.AddChild(IPopup popup, bool isSystemLayer)
        {
            popup.SetParent(
                isSystemLayer
                    ? systemLayerCanvas.transform
                    : defaultLayerCanvas.transform);
        }

        void IPopupCanvasController.AddChild(IPopupBackgroundInputBlocker popupBackgroundInputBlocker, bool isSystemLayer)
        {
            popupBackgroundInputBlocker.SetParent(
                isSystemLayer
                    ? systemLayerCanvas.transform
                    : defaultLayerCanvas.transform);
        }
    }
}