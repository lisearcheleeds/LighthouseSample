using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LighthouseExtends.UIComponent.Button
{
    public static class LHButtonHitChecker
    {
        static readonly List<RaycastResult> raycastResults = new();

        /// <summary>
        /// ボタン中央へ Raycast し、最前面にボタン自身（または子要素）が存在すれば true を返す。
        /// インタラクタブルでない場合、他の UI に隠れている場合は false。
        /// </summary>
        public static bool IsHittable(LHButton button)
        {
            if (!button.IsInteractable())
            {
                return false;
            }

            var canvas = button.GetComponentInParent<Canvas>();
            if (canvas == null)
            {
                return false;
            }

            var camera = canvas.renderMode == RenderMode.ScreenSpaceOverlay
                ? null
                : canvas.worldCamera;

            var screenPos = RectTransformUtility.WorldToScreenPoint(camera, button.transform.position);

            var eventData = new PointerEventData(EventSystem.current)
            {
                position = screenPos,
            };

            raycastResults.Clear();
            EventSystem.current.RaycastAll(eventData, raycastResults);

            if (raycastResults.Count == 0)
            {
                return false;
            }

            var topHit = raycastResults[0];
            return topHit.gameObject == button.gameObject
                || topHit.gameObject.transform.IsChildOf(button.transform);
        }
    }
}
