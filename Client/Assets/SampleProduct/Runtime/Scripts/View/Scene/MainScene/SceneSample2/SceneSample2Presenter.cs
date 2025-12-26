using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
using SampleProduct.Core;
using LighthouseExtends.TextTable;
using SampleProduct.View.Scene.MainScene.SceneSample3;
using SampleProduct.View.Scene.ModuleScene.Background;
using SampleProduct.View.Scene.ModuleScene.GlobalHeader;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.SceneSample2
{
    public class SceneSample2Presenter : ISceneSample2Presenter
    {
        ISceneSample2View sceneSample2View;
        ISampleSceneManager sceneManager;
        IGlobalHeaderModule globalHeaderModule;
        IBackgroundModule backgroundModule;

        [Inject]
        public void Construct(
            ISceneSample2View sceneSample2View,
            ISampleSceneManager sceneManager,
            IGlobalHeaderModule globalHeaderModule,
            IBackgroundModule backgroundModule)
        {
            this.sceneSample2View = sceneSample2View;
            this.sceneManager = sceneManager;
            this.globalHeaderModule = globalHeaderModule;
            this.backgroundModule = backgroundModule;
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
            globalHeaderModule.SetHeaderText(new TextData("SceneSample2GlobalHeader"));
            backgroundModule.SetBackgroundLayout(BackgroundLayout.Sample2Layout);
        }

        void OnClickChoice1()
        {
            sceneManager.TransitionScene(new SceneSample3Scene.SceneSample3TransitionData(1)).Forget();
        }

        void OnClickChoice2()
        {
            sceneManager.TransitionScene(new SceneSample3Scene.SceneSample3TransitionData(2)).Forget();
        }

        void OnClickChoice3()
        {
            sceneManager.TransitionScene(new SceneSample3Scene.SceneSample3TransitionData(3)).Forget();
        }

        bool ISceneSample2Presenter.TryClickBackButton() => sceneSample2View.TryClickBackButton();

        void OnClickBackScene()
        {
            sceneManager.BackScene().Forget();
        }
    }
}
