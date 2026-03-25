using System;

namespace SampleProduct.View.Scene.MainScene.SampleTop
{
    public interface ISampleTopView
    {
        public IDisposable SubscribeBackSceneButtonClick(Action action);

        public IDisposable SubscribeEditButtonClick(Action action);
        public IDisposable SubscribeGame1ButtonClick(Action action);
        public IDisposable SubscribeGame2ButtonClick(Action action);

        public IDisposable SubscribeSceneSample1ButtonClick(Action action);

        public IDisposable SubscribeDialogSample1ButtonClick(Action action);
        public IDisposable SubscribeDialogSample2ButtonClick(Action action);
    }
}