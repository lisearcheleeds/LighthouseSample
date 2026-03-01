using System.Threading;
using Cysharp.Threading.Tasks;
using LighthouseExtends.Popup;
using UnityEngine;
using VContainer;

namespace SampleProduct.View.Common.Popup.PopupTest
{
    public sealed class PopupSample2PopupPresenter : IPopupPresenter
    {
        IPopupModule popupModule;

        [Inject]
        public void Construct(IPopupModule popupModule)
        {
            this.popupModule = popupModule;
        }

        public void Bind(PopupSample2PopupView view, PopupSample2PopupData popupData)
        {
            view.SubscribeCloseButtonClick(OnClickCloseButton);
            view.SubscribeShowCodeButtonClick(OnClickShowCodeButton);
            view.SubscribeOpenPopup1ButtonClick(OnClickOpenPopup1Button);
            view.SubscribeOpenPopup2ButtonClick(OnClickOpenPopup2Button);
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
    }
}