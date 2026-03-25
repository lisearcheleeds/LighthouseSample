using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
using LighthouseExtends.ScreenStack;
using SampleProduct.View.Scene.MainScene.SampleTop.DialogSample1Dialog;
using SampleProduct.View.Scene.MainScene.SampleTop.DialogSample2Dialog;
using SampleProduct.View.Scene.MainScene.SceneSample1;
using SampleProduct.View.Scene.ModuleScene.Background;
using SampleProduct.View.Scene.ModuleScene.GlobalHeader;
using UnityEngine;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.SampleTop
{
    public sealed class SampleTopPresenter : ISampleTopPresenter
    {
        ISampleTopView sampleTopView;
        IScreenStackModule screenStackModule;
        ISceneManager sceneManager;
        IGlobalHeaderModule globalHeaderModule;
        IBackgroundModule backgroundModule;

        [Inject]
        public void Construct(
            ISampleTopView sampleTopView,
            IScreenStackModule screenStackModule,
            ISceneManager sceneManager,
            IGlobalHeaderModule globalHeaderModule,
            IBackgroundModule backgroundModule)
        {
            this.sampleTopView = sampleTopView;
            this.screenStackModule = screenStackModule;
            this.sceneManager = sceneManager;
            this.globalHeaderModule = globalHeaderModule;
            this.backgroundModule = backgroundModule;
        }

        void ISampleTopPresenter.Setup()
        {
            sampleTopView.SubscribeBackSceneButtonClick(OnClickBackScene);

            sampleTopView.SubscribeEditButtonClick(OnClickEditButton);
            sampleTopView.SubscribeGame1ButtonClick(OnClickGame1Button);
            sampleTopView.SubscribeGame2ButtonClick(OnClickGame2Button);

            sampleTopView.SubscribeSceneSample1ButtonClick(OnClickSceneSample1);

            sampleTopView.SubscribeDialogSample1ButtonClick(OnClickDialogSample1);
            sampleTopView.SubscribeDialogSample2ButtonClick(OnClickDialogSample2);
        }

        void ISampleTopPresenter.OnEnter()
        {
            globalHeaderModule.SetHeaderText("SampleTop");
            backgroundModule.SetBackgroundLayout(BackgroundLayout.SampleTop);
        }

        void OnClickBackScene()
        {
            sceneManager.BackScene();
        }

        void OnClickEditButton()
        {
            Debug.Log("OnClickEditButton");
        }

        void OnClickGame1Button()
        {
            Debug.Log("OnClickGame1Button");
        }

        void OnClickGame2Button()
        {
            Debug.Log("OnClickGame2Button");
        }

        void OnClickSceneSample1()
        {
            sceneManager.TransitionScene(new SceneSample1Scene.SceneSample1TransitionData());
        }

        void OnClickDialogSample1()
        {
            screenStackModule.Open(new DialogSample1Data(1)).Forget();
        }

        void OnClickDialogSample2()
        {
            screenStackModule.Open(new DialogSample2Data(1)).Forget();
        }
    }
}