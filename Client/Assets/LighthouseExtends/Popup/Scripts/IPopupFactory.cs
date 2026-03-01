using Cysharp.Threading.Tasks;

namespace LighthouseExtends.Popup
{
    public interface IPopupFactory
    {
        UniTask<IPopup> CreatePopup(IPopupData popupData);
    }
}