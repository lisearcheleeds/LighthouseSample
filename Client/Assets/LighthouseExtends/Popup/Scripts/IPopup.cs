using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LighthouseExtends.Popup
{
    public interface IPopup : IDisposable
    {
        void SetParent(Transform parentTransform);

        UniTask OnInitialize();
        UniTask InAnimation();
        UniTask OutAnimation();
    }
}