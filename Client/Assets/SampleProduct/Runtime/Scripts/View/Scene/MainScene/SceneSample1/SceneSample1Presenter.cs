using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
using SampleProduct.Core;
using LighthouseExtends.ScreenStack;
using LighthouseExtends.TextTable;
using SampleProduct.View.Scene.MainScene.SceneSample1.SceneTransitionDialog;
using SampleProduct.View.Scene.MainScene.SceneSample2;
using SampleProduct.View.Scene.ModuleScene.Background;
using SampleProduct.View.Scene.ModuleScene.GlobalHeader;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.SceneSample1
{
    public class SceneSample1Presenter : ISceneSample1Presenter
    {
        ISceneSample1View sceneSample1View;
        IScreenStackModule screenStackModule;
        ISampleSceneManager sceneManager;
        IGlobalHeaderModule globalHeaderModule;
        IBackgroundModule backgroundModule;

        [Inject]
        public void Construct(
            ISceneSample1View sceneSample1View,
            IScreenStackModule screenStackModule,
            ISampleSceneManager sceneManager,
            IGlobalHeaderModule globalHeaderModule,
            IBackgroundModule backgroundModule)
        {
            this.sceneSample1View = sceneSample1View;
            this.screenStackModule = screenStackModule;
            this.sceneManager = sceneManager;
            this.globalHeaderModule = globalHeaderModule;
            this.backgroundModule = backgroundModule;
        }

        void ISceneSample1Presenter.Setup()
        {
            sceneSample1View.SubscribeTransitionScene1ButtonClick(OnClickTransitionScene1);
            sceneSample1View.SubscribeTransitionScene2ButtonClick(OnClickTransitionScene2);
            sceneSample1View.SubscribeBackSceneButtonClick(OnClickBackScene);
        }

        void ISceneSample1Presenter.OnEnter()
        {
            globalHeaderModule.SetHeaderText(new TextData("SceneSample1GlobalHeader"));
            backgroundModule.SetBackgroundLayout(BackgroundLayout.Sample1Layout);
        }

        void OnClickTransitionScene1()
        {
            sceneManager.TransitionScene(new SceneSample2Scene.SceneSample2TransitionData()).Forget();
        }

        void OnClickTransitionScene2()
        {
            screenStackModule.Open(new SceneTransitionData()).Forget();
        }

        bool ISceneSample1Presenter.TryClickBackButton() => sceneSample1View.TryClickBackButton();

        void OnClickBackScene()
        {
            sceneManager.BackScene().Forget();
        }
    }
}
