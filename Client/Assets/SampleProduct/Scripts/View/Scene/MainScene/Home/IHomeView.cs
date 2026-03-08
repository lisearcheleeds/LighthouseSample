using System;

namespace SampleProduct.View.Scene.MainScene.Home
{
    public interface IHomeView
    {
        public IDisposable SubscribeEditButtonClick(Action action);
        public IDisposable SubscribeGame1ButtonClick(Action action);
        public IDisposable SubscribeGame2ButtonClick(Action action);

        public IDisposable SubscribeSceneSample1ButtonClick(Action action);
        public IDisposable SubscribeSceneSample2ButtonClick(Action action);

        public IDisposable SubscribePopupSample1ButtonClick(Action action);
        public IDisposable SubscribePopupSample2ButtonClick(Action action);
    }
}