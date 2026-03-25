using LighthouseExtends.UIComponent.Scripts.ExclusiveInput;
using UnityEngine.EventSystems;
using VContainer;

namespace LighthouseExtends.UIComponent.Scripts.Button
{
    public class LHButton : UnityEngine.UI.Button
    {
        IExclusiveInputService exclusiveInputService;
        bool isUsing;

        [Inject]
        public void Construct(IExclusiveInputService exclusiveInputService)
        {
            this.exclusiveInputService = exclusiveInputService;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
        }
    }
}