using Lighthouse.Scene;
using LighthouseExtends.Popup;
using SampleProduct.View.Scene.ModuleScene.GlobalHeader;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.SceneSample2
{
    public class SceneSample2Presenter : ISceneSample2Presenter
    {
        ISceneSample2View sceneSample2View;
        IPopupModule popupModule;
        ISceneManager sceneManager;
        IGlobalHeaderModule globalHeaderModule;

        [Inject]
        public void Construct(
            ISceneSample2View sceneSample2View,
            IPopupModule popupModule,
            ISceneManager sceneManager,
            IGlobalHeaderModule globalHeaderModule)
        {
            this.sceneSample2View = sceneSample2View;
            this.popupModule = popupModule;
            this.sceneManager = sceneManager;
            this.globalHeaderModule = globalHeaderModule;
        }

        void ISceneSample2Presenter.Setup()
        {
            sceneSample2View.SubscribeTransitionScene1ButtonClick(OnClickTransitionScene1);
            sceneSample2View.SubscribeTransitionScene2ButtonClick(OnClickTransitionScene2);
            sceneSample2View.SubscribeBackSceneButtonClick(OnClickBackScene);

            globalHeaderModule.SetHeaderText("Sample2");
        }

        void OnClickTransitionScene1()
        {
            sceneManager.TransitionScene(new SceneSample2Scene.SceneSample2TransitionData());
        }

        void OnClickTransitionScene2()
        {
            sceneManager.TransitionScene(new SceneSample2Scene.SceneSample2TransitionData());
        }

        void OnClickBackScene()
        {
            sceneManager.BackScene();
        }
    }
}