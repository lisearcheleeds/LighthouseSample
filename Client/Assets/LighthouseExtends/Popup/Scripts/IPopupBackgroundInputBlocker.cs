using UnityEngine;

namespace LighthouseExtends.Popup
{
    public interface IPopupBackgroundInputBlocker
    {
        void Setup();
        void SetParent(Transform parent);
        void BlockPopupBackground(bool isSystem);
        void UnBlock();
    }
}