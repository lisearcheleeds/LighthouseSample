using LighthouseExtends.Popup;

namespace SampleProduct.View.Common.Popup
{
    public sealed class PopupSample2PopupData : IPopupData
    {
        public bool IsSystem => false;
        public bool IsOverlayOpen => true;

        public int StackCount { get; }

        public PopupSample2PopupData(int stackCount)
        {
            StackCount = stackCount;
        }
    }
}