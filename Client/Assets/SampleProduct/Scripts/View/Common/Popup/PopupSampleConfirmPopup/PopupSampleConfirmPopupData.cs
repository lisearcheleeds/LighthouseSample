using System;
using LighthouseExtends.Popup;

namespace SampleProduct.View.Common.Popup
{
    public sealed class PopupSampleConfirmPopupData : IPopupData
    {
        public bool IsSystem => false;
        public bool IsOverlayOpen => true;

        public Action OnCloseAction { get; }

        public PopupSampleConfirmPopupData(Action onClose)
        {
            OnCloseAction = onClose;
        }
    }
}