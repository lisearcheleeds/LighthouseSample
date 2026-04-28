using Cysharp.Threading.Tasks;
using LighthouseExtends.TextTable;
using SampleProduct.Core;
using SampleProduct.View.Scene.ModuleScene.Background;
using SampleProduct.View.Scene.ModuleScene.GlobalHeader;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.SceneSample3
{
    public class SceneSample3Presenter : ISceneSample3Presenter
    {
        ISceneSample3View sceneSample3View;
        IProductSceneManager sceneManager;
        IGlobalHeaderModule globalHeaderModule;
        IBackgroundModule backgroundModule;

        [Inject]
        public void Construct(
            ISceneSample3View sceneSample3View,
            IProductSceneManager sceneManager,
            IGlobalHeaderModule globalHeaderModule,
            IBackgroundModule backgroundModule)
        {
            this.sceneSample3View = sceneSample3View;
            this.sceneManager = sceneManager;
            this.globalHeaderModule = globalHeaderModule;
            this.backgroundModule = backgroundModule;
        }

        void ISceneSample3Presenter.Setup()
        {
            sceneSample3View.SubscribeBackSceneButtonClick(OnClickBackScene);
        }

        void ISceneSample3Presenter.OnEnter()
        {
            globalHeaderModule.SetHeaderText(new TextData("SceneSample3GlobalHeader"));
            backgroundModule.SetBackgroundLayout(BackgroundLayout.Sample3Layout);
        }

        void ISceneSample3Presenter.ApplyChoiceData(int choiceData)
        {
            sceneSample3View.ApplyChoiceData(choiceData);
        }

        bool ISceneSample3Presenter.TryClickBackButton() => sceneSample3View.TryClickBackButton();

        void OnClickBackScene()
        {
            sceneManager.BackScene().Forget();
        }
    }
}
