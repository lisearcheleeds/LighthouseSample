using LighthouseExtends.ScreenStack;

namespace SampleProduct.View.Scene.MainScene.SampleTop.DialogSample1Dialog
{
    public sealed class DialogSample1Data : IScreenStackData
    {
        public bool IsSystem => false;
        public bool IsOverlayOpen => false;

        public int StackCount { get; }

        public DialogSample1Data(int stackCount)
        {
            StackCount = stackCount;
        }
    }
}