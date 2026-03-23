using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
using LighthouseExtends.Popup;
using SampleProduct.View.Scene.MainScene.SampleTop.PopupSample1Popup;
using SampleProduct.View.Scene.MainScene.SampleTop.PopupSample2Popup;
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
        IPopupModule popupModule;
        ISceneManager sceneManager;
        IGlobalHeaderModule globalHeaderModule;
        IBackgroundModule backgroundModule;

        [Inject]
        public void Construct(
            ISampleTopView sampleTopView,
            IPopupModule popupModule,
            ISceneManager sceneManager,
            IGlobalHeaderModule globalHeaderModule,
            IBackgroundModule backgroundModule)
        {
            this.sampleTopView = sampleTopView;
            this.popupModule = popupModule;
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

            sampleTopView.SubscribePopupSample1ButtonClick(OnClickPopupSample1);
            sampleTopView.SubscribePopupSample2ButtonClick(OnClickPopupSample2);
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

        void OnClickPopupSample1()
        {
            popupModule.OpenPopup(new PopupSample1PopupData(1)).Forget();
        }

        void OnClickPopupSample2()
        {
            popupModule.OpenPopup(new PopupSample2PopupData(1)).Forget();
        }
    }
}