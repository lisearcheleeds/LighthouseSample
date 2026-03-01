using System;

namespace SampleProduct.View.Scene.MainScene.Home
{
    public interface IHomeView
    {
        public IDisposable SubscribeEditButtonClick(Action action);
        public IDisposable SubscribeGame1ButtonClick(Action action);
        public IDisposable SubscribeGame2ButtonClick(Action action);

        public IDisposable SubscribeOptionButtonClick(Action action);

        public IDisposable SubscribePopupTest1ButtonClick(Action action);
        public IDisposable SubscribePopupTest2ButtonClick(Action action);
    }
}