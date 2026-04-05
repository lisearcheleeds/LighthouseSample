using LighthouseExtends.UIComponent.ExclusiveInput;
using UnityEngine.EventSystems;
using VContainer;

namespace LighthouseExtends.UIComponent.Button
{
    public class LHButton : UnityEngine.UI.Button
    {
        IExclusiveInputService exclusiveInputService;
        bool isUsing;
        int currentPointerId;

        [Inject]
        public void Construct(IExclusiveInputService exclusiveInputService)
        {
            this.exclusiveInputService = exclusiveInputService;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (!exclusiveInputService.TryUsePointerId(eventData.pointerId))
            {
                return;
            }

            isUsing = true;
            currentPointerId = eventData.pointerId;
            base.OnPointerDown(eventData);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            if (!isUsing)
            {
                return;
            }

            base.OnPointerUp(eventData);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (!isUsing)
            {
                return;
            }

            base.OnPointerClick(eventData);
            Release();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);

            if (isUsing)
            {
                Release();
            }
        }

        void OnApplicationPause(bool pause)
        {
            if (!pause || !isUsing)
            {
                return;
            }

            Release();
            InstantClearState();
        }

        void Release()
        {
            exclusiveInputService.ReleasePointerId(currentPointerId);
            isUsing = false;
        }
    }
}
