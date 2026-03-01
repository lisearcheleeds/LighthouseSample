using System.Threading;
using LighthouseExtends.Popup;
using SampleProduct.View.Common.Popup.PopupTest;
using UnityEngine;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.Home
{
    public sealed class HomeViewController : IHomeViewController
    {
        IHomeView homeView;
        IPopupModule popupModule;

        [Inject]
        public void Construct(IHomeView homeView, IPopupModule popupModule)
        {
            this.homeView = homeView;
            this.popupModule = popupModule;
        }

        void IHomeViewController.Setup()
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
            Debug.Log("OnClickPopupTest1");
            popupModule.OpenPopup(new PopupSample1PopupData(), CancellationToken.None);
        }

        void OnClickPopupTest2()
        {
            Debug.Log("OnClickPopupTest2");
            popupModule.OpenPopup(new PopupSample2PopupData(), CancellationToken.None);
        }
    }
}