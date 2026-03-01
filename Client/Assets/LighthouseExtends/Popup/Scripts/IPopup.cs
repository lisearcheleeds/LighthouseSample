using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LighthouseExtends.Popup
{
    public interface IPopup : IDisposable
    {
        void SetParent(Transform parentTransform);

        UniTask OnInitialize();

        void ResetInAnimation();
        UniTask PlayInAnimation();
        UniTask PlayOutAnimation();
        void EndOutAnimation();
    }
}