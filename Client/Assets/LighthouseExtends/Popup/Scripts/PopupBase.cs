using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LighthouseExtends.Popup
{
    public abstract class PopupBase : MonoBehaviour, IPopup
    {
        public virtual UniTask OnInitialize()
        {
            return UniTask.CompletedTask;
        }

        public void SetParent(Transform parentTransform)
        {
            transform.SetParent(parentTransform, false);
        }

        public virtual UniTask InAnimation()
        {
            return UniTask.CompletedTask;
        }

        public virtual UniTask OutAnimation()
        {
            return UniTask.CompletedTask;
        }

        public virtual void Dispose()
        {
            Destroy(gameObject);
        }
    }
}