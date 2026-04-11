using System;
using LighthouseExtends.Font;
using LighthouseExtends.TextTable;
using R3;
using TMPro;
using UnityEngine;

namespace LighthouseExtends.UIComponent.TextMeshPro
{
    public class LHTextMeshPro : TextMeshProUGUI
    {
        [SerializeField] string textKey;

        ITextData textData;
        IDisposable textSubscription;
        IDisposable fontSubscription;

        public void SetTextData(ITextData textData)
        {
            this.textData = textData;
            if (isActiveAndEnabled)
            {
                ApplyText();
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (!Application.isPlaying)
            {
                return;
            }

            var textTableService = TextTableService.Instance;
            if (textTableService == null)
            {
                ApplyText();
                return;
            }

            textSubscription = textTableService.CurrentLanguage.Subscribe(_ => ApplyText());

            var fontService = FontService.Instance;
            if (fontService != null)
            {
                fontSubscription = fontService.CurrentFont.Subscribe(ApplyFont);
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            textSubscription?.Dispose();
            textSubscription = null;

            fontSubscription?.Dispose();
            fontSubscription = null;
        }

        void ApplyText()
        {
            var data = textData ?? (!string.IsNullOrEmpty(textKey) ? new TextData(textKey) : null);
            if (data == null)
            {
                return;
            }

            var service = TextTableService.Instance;
            text = service != null ? service.GetText(data) : data.TextKey;
        }

        void ApplyFont(TMP_FontAsset fontAsset)
        {
            if (fontAsset != null)
            {
                font = fontAsset;
            }
        }
    }
}
