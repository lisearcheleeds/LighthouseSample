using System;

namespace SampleProduct.View.Scene.MainScene.Title
{
    public interface ITitleView
    {
        public IDisposable SubscribeScreenButtonClick(Action action);
    }
}