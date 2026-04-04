using System;

namespace SampleProduct.View.Scene.MainScene.Home
{
    public interface IHomeView
    {
        public IDisposable SubscribeGuideButtonClick(Action action);
        public IDisposable SubscribeGithubButtonClick(Action action);
        public IDisposable SubscribeSnsButtonClick(Action action);

        public IDisposable SubscribePhilosophyButtonClick(Action action);
        public IDisposable SubscribeSampleButtonClick(Action action);
        public IDisposable SubscribeRequireToolsButtonClick(Action action);
    }
}