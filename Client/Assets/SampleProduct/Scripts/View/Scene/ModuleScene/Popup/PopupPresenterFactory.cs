using LighthouseExtends.Popup;
using SampleProduct.View.Common.Popup.PopupTest;

namespace SampleProduct.View.Scene.ModuleScene.Popup
{
    public sealed class PopupPresenterFactory : IPopupPresenterFactory
    {
        IPopupPresenter IPopupPresenterFactory.CreatePopupPresenter(IPopupData popupData)
        {
            return popupData switch
            {
                PopupTest1Data => new PopupTest1Presenter(),
            };
        }
    }
}