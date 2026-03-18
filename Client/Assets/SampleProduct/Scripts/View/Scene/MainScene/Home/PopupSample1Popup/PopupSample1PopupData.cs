using LighthouseExtends.Popup;

namespace SampleProduct.View.Scene.MainScene.Home.PopupSample1Popup
{
    public sealed class PopupSample1PopupData : IPopupData
    {
        public bool IsSystem => false;
        public bool IsOverlayOpen => false;

        public int StackCount { get; }

        public PopupSample1PopupData(int stackCount)
        {
            StackCount = stackCount;
        }
    }
}