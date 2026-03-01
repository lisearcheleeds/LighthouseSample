namespace LighthouseExtends.Popup
{
    public sealed class PopupEntity
    {
        public IPopup Popup { get; }
        public IPopupPresenter PopupPresenter { get; }

        public PopupEntity(IPopup popup, IPopupPresenter popupPresenter)
        {
            Popup = popup;
            PopupPresenter = popupPresenter;
        }
    }
}