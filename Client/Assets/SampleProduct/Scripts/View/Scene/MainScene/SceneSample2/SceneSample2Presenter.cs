using Lighthouse.Scene;
using SampleProduct.View.Scene.MainScene.SceneSample3;
using SampleProduct.View.Scene.ModuleScene.GlobalHeader;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.SceneSample2
{
    public class SceneSample2Presenter : ISceneSample2Presenter
    {
        ISceneSample2View sceneSample2View;
        ISceneManager sceneManager;
        IGlobalHeaderModule globalHeaderModule;

        [Inject]
        public void Construct(
            ISceneSample2View sceneSample2View,
            ISceneManager sceneManager,
            IGlobalHeaderModule globalHeaderModule)
        {
            this.sceneSample2View = sceneSample2View;
            this.sceneManager = sceneManager;
            this.globalHeaderModule = globalHeaderModule;
        }

        void ISceneSample2Presenter.Setup()
        {
            sceneSample2View.SubscribeChoice1ButtonClick(OnClickChoice1);
            sceneSample2View.SubscribeChoice2ButtonClick(OnClickChoice2);
            sceneSample2View.SubscribeChoice3ButtonClick(OnClickChoice3);

            sceneSample2View.SubscribeBackSceneButtonClick(OnClickBackScene);
        }

        void ISceneSample2Presenter.OnEnter()
        {
            globalHeaderModule.SetHeaderText("Home > SampleScene1 > SampleScene2");
        }

        void OnClickChoice1()
        {
            sceneManager.TransitionScene(new SceneSample3Scene.SceneSample3TransitionData(1));
        }

        void OnClickChoice2()
        {
            sceneManager.TransitionScene(new SceneSample3Scene.SceneSample3TransitionData(2));
        }

        void OnClickChoice3()
        {
            sceneManager.TransitionScene(new SceneSample3Scene.SceneSample3TransitionData(3));
        }

        void OnClickBackScene()
        {
            sceneManager.BackScene();
        }
    }
}