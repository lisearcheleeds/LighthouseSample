using System.Threading;
using LighthouseExtends.Popup;
using SampleProduct.View.Popup.PopupSample1Popup;
using SampleProduct.View.Popup.PopupSample2Popup;
using UnityEngine;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.Home
{
    public sealed class HomePresenter : IHomePresenter
    {
        IHomeView homeView;
        IPopupModule popupModule;

        [Inject]
        public void Construct(IHomeView homeView, IPopupModule popupModule)
        {
            this.homeView = homeView;
            this.popupModule = popupModule;
        }

        void IHomePresenter.Setup()
        {
            homeView.SubscribeEditButtonClick(OnClickEditButton);
            homeView.SubscribeGame1ButtonClick(OnClickGame1Button);
            homeView.SubscribeGame2ButtonClick(OnClickGame2Button);
            homeView.SubscribeOptionButtonClick(OnClickOption);
            homeView.SubscribePopupTest1ButtonClick(OnClickPopupTest1);
            homeView.SubscribePopupTest2ButtonClick(OnClickPopupTest2);
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

        void OnClickOption()
        {
            Debug.Log("OnClickOption");
        }

        void OnClickPopupTest1()
        {
            popupModule.OpenPopup(new PopupSample1PopupData(1), CancellationToken.None);
        }

        void OnClickPopupTest2()
        {
            popupModule.OpenPopup(new PopupSample2PopupData(1), CancellationToken.None);
        }
    }
}