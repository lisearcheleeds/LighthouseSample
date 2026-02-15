using System;
using Cysharp.Threading.Tasks;
using Lighthouse.Scene;

namespace LighthouseExtends.Popup
{
    public interface IPopup : IDisposable
    {
        IPopupData PopupData { get; }

        UniTask OnInitialize();
        UniTask OnEnter(IPopupData popupData);
        UniTask InAnimation();
        UniTask OutAnimation();
        UniTask OnLeave();
    }
}