using System.Threading;
using Cysharp.Threading.Tasks;
using LighthouseExtends.Popup;
using SampleProduct.View.Scene.MainScene.Home.PopupSample2Popup;
using SampleProduct.View.Scene.MainScene.Home.PopupSampleConfirmPopup;
using UnityEngine;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.Home.PopupSample1Popup
{
    public sealed class PopupSample1PopupPresenter : IPopupPresenter
    {
        IPopupModule popupModule;

        PopupSample1PopupView popupView;
        PopupSample1PopupData popupData;

        [Inject]
        public void Construct(IPopupModule popupModule)
        {
            this.popupModule = popupModule;
        }

        public void Bind(PopupSample1PopupView popupView, PopupSample1PopupData popupData)
        {
            popupView.SubscribeCloseButtonClick(OnClickCloseButton);
            popupView.SubscribeShowCodeButtonClick(OnClickShowCodeButton);
            popupView.SubscribeOpenPopup1ButtonClick(OnClickOpenPopup1Button);
            popupView.SubscribeOpenPopup2ButtonClick(OnClickOpenPopup2Button);
            popupView.SubscribeConfirmOpenPopupButtonClick(OnClickOpenConfirmPopupButton);

            this.popupView = popupView;
            this.popupData = popupData;
        }

        UniTask IPopupPresenter.OnEnter(bool isResume)
        {
            popupView.SetText($"StackCount: {popupData.StackCount}");

            return UniTask.CompletedTask;
        }

        UniTask IPopupPresenter.OnLeave()
        {
            return UniTask.CompletedTask;
        }

        void OnClickCloseButton()
        {
            popupModule.ClosePopup().Forget();
        }

        void OnClickShowCodeButton()
        {
            Application.OpenURL("https://github.com/lisearcheleeds/LighthouseFramework");
        }

        void OnClickOpenPopup1Button()
        {
            popupModule.OpenPopup(new PopupSample1PopupData(popupData.StackCount + 1)).Forget();
        }

        void OnClickOpenPopup2Button()
        {
            popupModule.OpenPopup(new PopupSample2PopupData(popupData.StackCount + 1)).Forget();
        }

        void OnClickOpenConfirmPopupButton()
        {
            popupModule.OpenPopup(new PopupSampleConfirmPopupData(() => popupModule.ClosePopup().Forget())).Forget();
        }
    }
}