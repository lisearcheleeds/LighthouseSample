using System;
using LighthouseExtends.ScreenStack;

namespace SampleProduct.View.Scene.MainScene.SampleTop.DialogSampleConfirmDialog
{
    public sealed class DialogSampleConfirmData : IScreenStackData
    {
        public bool IsSystem => false;
        public bool IsOverlayOpen => true;

        public Action OnCloseAction { get; }

        public DialogSampleConfirmData(Action onClose)
        {
            OnCloseAction = onClose;
        }
    }
}