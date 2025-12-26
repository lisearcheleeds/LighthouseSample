using System;

namespace SampleProduct.View.Scene.MainScene.SceneSample3
{
    public interface ISceneSample3View
    {
        IDisposable SubscribeBackSceneButtonClick(Action action);
        bool TryClickBackButton();

        void ApplyChoiceData(int choiceData);
    }
}