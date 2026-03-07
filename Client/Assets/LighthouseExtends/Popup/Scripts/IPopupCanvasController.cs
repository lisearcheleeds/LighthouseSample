namespace LighthouseExtends.Popup
{
    public interface IPopupCanvasController
    {
        public void AddChild(IPopup popup, bool isSystemLayer);
        public void AddChild(IPopupBackgroundInputBlocker popupBackgroundInputBlocker, bool isSystemLayer);
    }
}