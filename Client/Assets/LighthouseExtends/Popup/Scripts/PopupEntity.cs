namespace LighthouseExtends.Popup
{
    public sealed class PopupEntity
    {
        public IPopup Popup { get; }
        public IPopupPresenter PopupPresenter { get; }
        public IPopupData PopupData { get; }

        public PopupEntity(IPopup popup, IPopupPresenter popupPresenter, IPopupData popupData)
        {
            Popup = popup;
            PopupPresenter = popupPresenter;
            PopupData = popupData;
        }
    }
}