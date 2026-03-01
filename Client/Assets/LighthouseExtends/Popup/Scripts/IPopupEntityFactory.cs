using System.Threading;
using Cysharp.Threading.Tasks;

namespace LighthouseExtends.Popup
{
    public interface IPopupEntityFactory
    {
        UniTask<PopupEntity> CreateAsync(IPopupData data, CancellationToken ct);
    }
}