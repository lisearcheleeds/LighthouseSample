using System.Threading;
using Cysharp.Threading.Tasks;
using LighthouseExtends.Popup;
using UnityEngine;
using VContainer;

namespace SampleProduct.View.Common.Popup
{
    public sealed class PopupSample1PopupPresenter : IPopupPresenter
    {
        IPopupModule popupModule;

        [Inject]
        public void Construct(IPopupModule popupModule)
        {
            this.popupModule = popupModule;
        }

        public void Bind(PopupSample1PopupView view, PopupSample1PopupData popupData)
        {
            view.SubscribeCloseButtonClick(OnClickCloseButton);
            view.SubscribeShowCodeButtonClick(OnClickShowCodeButton);
            view.SubscribeOpenPopup1ButtonClick(OnClickOpenPopup1Button);
            view.SubscribeOpenPopup2ButtonClick(OnClickOpenPopup2Button);
            view.SubscribeConfirmOpenPopupButtonClick(OnClickOpenConfirmPopupButton);
        }

        UniTask IPopupPresenter.OnEnter(bool isResume)
        {
            return UniTask.CompletedTask;
        }

        UniTask IPopupPresenter.OnLeave()
        {
            return UniTask.CompletedTask;
        }

        void OnClickCloseButton()
        {
            popupModule.ClosePopup(CancellationToken.None);
        }

        void OnClickShowCodeButton()
        {
            Application.OpenURL("https://github.com/lisearcheleeds/LighthouseFramework");
        }

        void OnClickOpenPopup1Button()
        {
            popupModule.OpenPopup(new PopupSample1PopupData(), CancellationToken.None);
        }

        void OnClickOpenPopup2Button()
        {
            popupModule.OpenPopup(new PopupSample2PopupData(), CancellationToken.None);
        }

        void OnClickOpenConfirmPopupButton()
        {
            popupModule.OpenPopup(new PopupSampleConfirmPopupData(() => popupModule.ClosePopup(CancellationToken.None).Forget()), CancellationToken.None);
        }
    }
}