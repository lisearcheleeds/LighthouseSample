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

        public virtual void ResetInAnimation()
        {
        }

        public virtual UniTask PlayInAnimation()
        {
            return UniTask.CompletedTask;
        }

        public virtual UniTask PlayOutAnimation()
        {
            return UniTask.CompletedTask;
        }

        public virtual void EndOutAnimation()
        {a
        }

        public virtual void Dispose()
        {
            Destroy(gameObject);
        }
    }
}