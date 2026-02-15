using Cysharp.Threading.Tasks;

namespace LighthouseExtends.Popup
{
    public interface IPopupAssetLoader
    {
        UniTask<IPopup> LoadPopupAsset(IPopupAddress popupAddress);
    }
}