using System.Threading;
using Lighthouse.Scene;
using LighthouseExtends.Popup;
using SampleProduct.View.Popup.PopupSample1Popup;
using SampleProduct.View.Popup.PopupSample2Popup;
using SampleProduct.View.Scene.MainScene.SceneSample1;
using SampleProduct.View.Scene.MainScene.SceneSample2;
using SampleProduct.View.Scene.ModuleScene.GlobalHeader;
using UnityEngine;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.Home
{
    public sealed class HomePresenter : IHomePresenter
    {
        IHomeView homeView;
        IPopupModule popupModule;
        ISceneManager sceneManager;
        IGlobalHeaderModule globalHeaderModule;

        [Inject]
        public void Construct(
            IHomeView homeView,
            IPopupModule popupModule,
            ISceneManager sceneManager,
            IGlobalHeaderModule globalHeaderModule)
        {
            this.homeView = homeView;
            this.popupModule = popupModule;
            this.sceneManager = sceneManager;
            this.globalHeaderModule = globalHeaderModule;
        }

        void IHomePresenter.Setup()
        {
            homeView.SubscribeEditButtonClick(OnClickEditButton);
            homeView.SubscribeGame1ButtonClick(OnClickGame1Button);
            homeView.SubscribeGame2ButtonClick(OnClickGame2Button);
            homeView.SubscribeSceneSample1ButtonClick(OnClickSceneSample1);
            homeView.SubscribeSceneSample2ButtonClick(OnClickSceneSample2);
            homeView.SubscribePopupSample1ButtonClick(OnClickPopupSample1);
            homeView.SubscribePopupSample2ButtonClick(OnClickPopupSample2);

            globalHeaderModule.SetHeaderText("Home");
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

        void OnClickSceneSample2()
        {
            sceneManager.TransitionScene(new SceneSample2Scene.SceneSample2TransitionData());
        }

        void OnClickPopupSample1()
        {
            popupModule.OpenPopup(new PopupSample1PopupData(1), CancellationToken.None);
        }

        void OnClickPopupSample2()
        {
            popupModule.OpenPopup(new PopupSample2PopupData(1), CancellationToken.None);
        }
    }
}