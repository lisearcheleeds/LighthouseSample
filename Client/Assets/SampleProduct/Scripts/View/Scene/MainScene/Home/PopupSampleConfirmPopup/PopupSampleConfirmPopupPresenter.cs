using System.Threading;
using Cysharp.Threading.Tasks;
using LighthouseExtends.Popup;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.Home.PopupSampleConfirmPopup
{
    public sealed class PopupSampleConfirmPopupPresenter : IPopupPresenter
    {
        IPopupModule popupModule;
        PopupSampleConfirmPopupData popupData;

        [Inject]
        public void Construct(IPopupModule popupModule)
        {
            this.popupModule = popupModule;
        }

        public void Bind(PopupSampleConfirmPopupView view, PopupSampleConfirmPopupData popupData)
        {
            this.popupData = popupData;

            view.SubscribeCloseButtonClick(OnClickCloseButton);
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
            popupData.OnCloseAction();
            popupModule.ClosePopup(CancellationToken.None).Forget();
        }
    }
}