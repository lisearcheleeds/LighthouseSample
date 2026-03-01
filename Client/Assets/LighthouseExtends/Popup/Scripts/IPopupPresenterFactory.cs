namespace LighthouseExtends.Popup
{
    public interface IPopupPresenterFactory
    {
        IPopupPresenter CreatePopupPresenter(IPopupData popupData);
    }
}