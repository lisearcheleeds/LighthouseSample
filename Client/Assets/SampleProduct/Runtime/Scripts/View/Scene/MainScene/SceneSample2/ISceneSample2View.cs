using System;

namespace SampleProduct.View.Scene.MainScene.SceneSample2
{
    public interface ISceneSample2View
    {
        IDisposable SubscribeChoice1ButtonClick(Action action);
        IDisposable SubscribeChoice2ButtonClick(Action action);
        IDisposable SubscribeChoice3ButtonClick(Action action);

        IDisposable SubscribeBackSceneButtonClick(Action action);
        bool TryClickBackButton();
    }
}