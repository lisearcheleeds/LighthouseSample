using LighthouseExtends.Popup;
using UnityEngine;

namespace SampleProduct.View.Popup.PopupSampleConfirmPopup
{
    public sealed class PopupSampleConfirmPopup : StandardPopupBase, IPopupSetup<PopupSampleConfirmPopupPresenter, PopupSampleConfirmPopupData>
    {
        [SerializeField] PopupSampleConfirmPopupView popupSampleConfirmPopupView;

        public void Setup(PopupSampleConfirmPopupPresenter popupPresenter, PopupSampleConfirmPopupData popupData)
        {
            popupPresenter.Bind(popupSampleConfirmPopupView, popupData);
        }
    }
}