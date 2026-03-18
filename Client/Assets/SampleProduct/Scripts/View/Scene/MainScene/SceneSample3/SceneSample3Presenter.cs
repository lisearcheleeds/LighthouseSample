using Lighthouse.Scene;
using SampleProduct.View.Scene.ModuleScene.GlobalHeader;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.SceneSample3
{
    public class SceneSample3Presenter : ISceneSample3Presenter
    {
        ISceneSample3View sceneSample3View;
        ISceneManager sceneManager;
        IGlobalHeaderModule globalHeaderModule;

        [Inject]
        public void Construct(
            ISceneSample3View sceneSample3View,
            ISceneManager sceneManager,
            IGlobalHeaderModule globalHeaderModule)
        {
            this.sceneSample3View = sceneSample3View;
            this.sceneManager = sceneManager;
            this.globalHeaderModule = globalHeaderModule;
        }

        void ISceneSample3Presenter.Setup()
        {
            sceneSample3View.SubscribeBackSceneButtonClick(OnClickBackScene);
        }

        void ISceneSample3Presenter.OnEnter()
        {
            globalHeaderModule.SetHeaderText("Home > SampleScene1 > SampleScene2 > SampleScene3");
        }

        void ISceneSample3Presenter.ApplyChoiceData(int choiceData)
        {
            sceneSample3View.ApplyChoiceData(choiceData);
        }

        void OnClickBackScene()
        {
            sceneManager.BackScene();
        }
    }
}