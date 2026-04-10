using System;
using LighthouseExtends.Language;
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
        IDisposable languageSubscription;

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

            var languageService = LanguageService.Instance;
            if (languageService == null)
            {
                ApplyText();
                return;
            }

            languageSubscription = languageService.CurrentLanguage.Subscribe(_ => ApplyText());
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            languageSubscription?.Dispose();
            languageSubscription = null;
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
    }
}
