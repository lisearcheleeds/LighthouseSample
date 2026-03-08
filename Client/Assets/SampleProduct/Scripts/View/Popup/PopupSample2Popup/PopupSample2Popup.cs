using LighthouseExtends.Popup;
using UnityEngine;

namespace SampleProduct.View.Popup.PopupSample2Popup
{
    public sealed class PopupSample2Popup : StandardPopupBase, IPopupSetup<PopupSample2PopupPresenter, PopupSample2PopupData>
    {
        [SerializeField] PopupSample2PopupView popupSample2PopupView;

        public void Setup(PopupSample2PopupPresenter popupPresenter, PopupSample2PopupData popupData)
        {
            popupPresenter.Bind(popupSample2PopupView, popupData);
        }
    }
}