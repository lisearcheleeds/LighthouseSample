using Cysharp.Threading.Tasks;
using LighthouseExtends.Popup;
using VContainer;

namespace SampleProduct.View.Common.Popup.PopupTest
{
    public sealed class PopupSample1PopupPresenter : IPopupPresenter
    {
        [Inject]
        public void Construct()
        {
        }

        public void Bind(PopupSample1PopupView view, PopupSample1PopupData popupData)
        {
        }

        UniTask IPopupPresenter.OnEnter(bool isResume)
        {
            return UniTask.CompletedTask;
        }

        public UniTask OnLeave()
        {
            return UniTask.CompletedTask;
        }
    }
}