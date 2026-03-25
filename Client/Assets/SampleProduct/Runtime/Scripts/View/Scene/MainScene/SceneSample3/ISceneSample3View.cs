using System;

namespace SampleProduct.View.Scene.MainScene.SceneSample3
{
    public interface ISceneSample3View
    {
        IDisposable SubscribeBackSceneButtonClick(Action action);

        void ApplyChoiceData(int choiceData);
    }
}