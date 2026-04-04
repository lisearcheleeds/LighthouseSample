using LighthouseExtends.ScreenStack;

namespace SampleProduct.View.Scene.MainScene.SampleTop.DialogSample2Dialog
{
    public sealed class DialogSample2Data : IScreenStackData
    {
        public bool IsSystem => false;
        public bool IsOverlayOpen => true;

        public int StackCount { get; }

        public DialogSample2Data() : this(1)
        {
        }

        public DialogSample2Data(int stackCount)
        {
            StackCount = stackCount;
        }
    }
}