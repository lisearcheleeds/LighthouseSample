using LighthouseExtends.ScreenStack;

namespace SampleProduct.OverlayElement
{
    public sealed class OverlayElementData : IScreenStackData
    {
        public bool IsSystem => false;
        public bool IsOverlayOpen => false;
    }
}
