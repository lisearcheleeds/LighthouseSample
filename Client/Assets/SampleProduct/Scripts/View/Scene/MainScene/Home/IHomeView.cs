using System;

namespace SampleProduct.View.Scene.MainScene.Home
{
    public interface IHomeView
    {
        public IDisposable SubscribeEditButtonButtonClick(Action action);
        public IDisposable SubscribeGame1ButtonButtonClick(Action action);
        public IDisposable SubscribeGame2ButtonButtonClick(Action action);

        public IDisposable SubscribeOptionButtonClick(Action action);

        public IDisposable SubscribePopupTest1ButtonClick(Action action);
        public IDisposable SubscribePopupTest2ButtonClick(Action action);
    }
}