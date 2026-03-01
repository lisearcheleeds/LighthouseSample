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
            homeView.SubscribeEditButtonButtonClick(OnClickEditButton);
            homeView.SubscribeGame1ButtonButtonClick(OnClickGame1Button);
            homeView.SubscribeGame2ButtonButtonClick(OnClickGame2Button);
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
            popupModule.OpenPopup(new PopupTest1Data(), CancellationToken.None);
        }

        void OnClickPopupTest2()
        {
            Debug.Log("OnClickPopupTest2");
        }
    }
}