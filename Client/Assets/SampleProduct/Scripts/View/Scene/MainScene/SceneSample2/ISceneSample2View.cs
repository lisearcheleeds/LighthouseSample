using System;

namespace SampleProduct.View.Scene.MainScene.SceneSample2
{
    public interface ISceneSample2View
    {
        IDisposable SubscribeTransitionScene1ButtonClick(Action action);
        IDisposable SubscribeTransitionScene2ButtonClick(Action action);
        IDisposable SubscribeBackSceneButtonClick(Action action);
    }
}