using LighthouseExtends.Popup;

namespace Lighthouse.Scene
{
    public interface IPopupData
    {
        public IPopupAddress PopupAddress { get; }
        public bool IsSystem { get; }
        public bool IsKeepView { get; }
    }
}