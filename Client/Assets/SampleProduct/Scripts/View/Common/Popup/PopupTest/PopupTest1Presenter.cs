using Cysharp.Threading.Tasks;
using LighthouseExtends.Popup;

namespace SampleProduct.View.Common.Popup.PopupTest
{
    public sealed class PopupTest1Presenter : IPopupPresenter
    {
        public IPopupData PopupData { get; private set; }

        public UniTask OnEnter(IPopupData popupData, IPopup popup, bool isResume)
        {
            PopupData = popupData;
            return UniTask.CompletedTask;
        }

        public UniTask OnLeave()
        {
            return UniTask.CompletedTask;
        }
    }
}