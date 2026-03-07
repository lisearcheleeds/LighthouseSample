using LighthouseExtends.UIComponent.Scripts.RaycastTargetObject;
using UnityEngine;
using VContainer;

namespace LighthouseExtends.Popup
{
    public sealed class PopupBackgroundInputBlocker : MonoBehaviour, IPopupBackgroundInputBlocker
    {
        [SerializeField] LHRaycastTargetObject raycastTargetObject;

        IPopupCanvasController popupCanvasController;

        RectTransform rectTransform;

        [Inject]
        public void Construct(IPopupCanvasController popupCanvasController)
        {
            this.popupCanvasController = popupCanvasController;
            rectTransform = (RectTransform)transform;
        }

        void IPopupBackgroundInputBlocker.Setup()
        {
            raycastTargetObject.gameObject.SetActive(false);
            popupCanvasController.AddChild(this, false);

        }

        void IPopupBackgroundInputBlocker.SetParent(Transform parent)
        {
            transform.SetParent(parent);
        }

        void IPopupBackgroundInputBlocker.BlockPopupBackground(bool isSystem)
        {
            raycastTargetObject.gameObject.SetActive(true);
            popupCanvasController.AddChild(this, isSystem);
            raycastTargetObject.transform.SetSiblingIndex(Mathf.Max(0, raycastTargetObject.transform.parent.childCount - 2));

            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.sizeDelta = Vector2.zero;
            rectTransform.anchoredPosition = Vector2.zero;
        }

        void IPopupBackgroundInputBlocker.UnBlock()
        {
            raycastTargetObject.gameObject.SetActive(false);
        }
    }
}