using System.Threading;
using Cysharp.Threading.Tasks;
using LighthouseExtends.Popup;
using SampleProduct.View.Scene.MainScene.Home.PopupSample1Popup;
using SampleProduct.View.Scene.MainScene.Home.PopupSampleConfirmPopup;
using UnityEngine;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.Home.PopupSample2Popup
{
    public sealed class PopupSample2PopupPresenter : IPopupPresenter
    {
        IPopupModule popupModule;

        PopupSample2PopupView popupView;
        PopupSample2PopupData popupData;

        [Inject]
        public void Construct(IPopupModule popupModule)
        {
            this.popupModule = popupModule;
        }

        public void Bind(PopupSample2PopupView popupView, PopupSample2PopupData popupData)
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
            popupModule.ClosePopup(CancellationToken.None).Forget();
        }

        void OnClickShowCodeButton()
        {
            Application.OpenURL("https://github.com/lisearcheleeds/LighthouseFramework");
        }

        void OnClickOpenPopup1Button()
        {
            popupModule.OpenPopup(new PopupSample1PopupData(popupData.StackCount + 1), CancellationToken.None).Forget();
        }

        void OnClickOpenPopup2Button()
        {
            popupModule.OpenPopup(new PopupSample2PopupData(popupData.StackCount + 1), CancellationToken.None).Forget();
        }

        void OnClickOpenConfirmPopupButton()
        {
            popupModule.OpenPopup(new PopupSampleConfirmPopupData(() => popupModule.ClosePopup(CancellationToken.None).Forget()), CancellationToken.None).Forget();
        }
    }
}