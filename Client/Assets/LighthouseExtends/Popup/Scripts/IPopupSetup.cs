namespace LighthouseExtends.Popup
{
    public interface IPopupSetup<in TPopupPresenter, in TPopupData>
    {
        void Setup(TPopupPresenter popupPresenter, TPopupData popupData);
    }
}