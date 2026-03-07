using System.Threading;
using Cysharp.Threading.Tasks;

namespace LighthouseExtends.Popup
{
    public interface IPopupInstanceFactory
    {
        UniTask<TPopup> CreatePopupInstance<TPopup>(string popupAddress, CancellationToken ct);
    }
}