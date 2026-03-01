using LighthouseExtends.Popup;
using UnityEngine;

namespace SampleProduct.View.Common.Popup.PopupTest
{
    public sealed class PopupSample1Popup : StandardPopupBase, IPopupSetup<PopupSample1PopupPresenter, PopupSample1PopupData>
    {
        [SerializeField] PopupSample1PopupView popupSample1PopupView;

        public void Setup(PopupSample1PopupPresenter popupPresenter, PopupSample1PopupData popupData)
        {
            popupPresenter.Bind(popupSample1PopupView, popupData);
        }
    }
}