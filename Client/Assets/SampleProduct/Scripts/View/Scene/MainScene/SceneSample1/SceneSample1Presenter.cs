using Lighthouse.Scene;
using LighthouseExtends.Popup;
using SampleProduct.View.Scene.MainScene.SceneSample2;
using SampleProduct.View.Scene.ModuleScene.GlobalHeader;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.SceneSample1
{
    public class SceneSample1Presenter : ISceneSample1Presenter
    {
        ISceneSample1View sceneSample1View;
        IPopupModule popupModule;
        ISceneManager sceneManager;
        IGlobalHeaderModule globalHeaderModule;

        [Inject]
        public void Construct(
            ISceneSample1View sceneSample1View,
            IPopupModule popupModule,
            ISceneManager sceneManager,
            IGlobalHeaderModule globalHeaderModule)
        {
            this.sceneSample1View = sceneSample1View;
            this.popupModule = popupModule;
            this.sceneManager = sceneManager;
            this.globalHeaderModule = globalHeaderModule;
        }

        void ISceneSample1Presenter.Setup()
        {
            sceneSample1View.SubscribeTransitionScene1ButtonClick(OnClickTransitionScene1);
            sceneSample1View.SubscribeTransitionScene2ButtonClick(OnClickTransitionScene2);
            sceneSample1View.SubscribeBackSceneButtonClick(OnClickBackScene);

            globalHeaderModule.SetHeaderText("Home");
        }

        void OnClickTransitionScene1()
        {
            sceneManager.TransitionScene(new SceneSample1Scene.SceneSample1TransitionData());
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