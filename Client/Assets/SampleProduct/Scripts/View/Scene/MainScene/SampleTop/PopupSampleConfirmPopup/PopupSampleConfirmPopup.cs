using LighthouseExtends.Popup;
using SampleProduct.View.Popup;
using UnityEngine;

namespace SampleProduct.View.Scene.MainScene.SampleTop.PopupSampleConfirmPopup
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