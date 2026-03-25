using System;

namespace SampleProduct.View.Scene.MainScene.SceneSample1
{
    public interface ISceneSample1View
    {
        IDisposable SubscribeTransitionScene1ButtonClick(Action action);
        IDisposable SubscribeTransitionScene2ButtonClick(Action action);
        IDisposable SubscribeBackSceneButtonClick(Action action);
    }
}