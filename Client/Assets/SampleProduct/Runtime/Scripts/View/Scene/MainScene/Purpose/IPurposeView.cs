using System;

namespace SampleProduct.View.Scene.MainScene.Purpose
{
    public interface IPurposeView
    {
        public IDisposable SubscribeBackSceneButtonClick(Action action);
        public bool TryClickBackButton();
    }
}
