using LighthouseExtends.ScreenStack;

namespace SampleProduct.TextView
{
    public sealed class TextViewElementData : IScreenStackData
    {
        public bool IsSystem => false;
        public bool IsOverlayOpen => false;
    }
}
