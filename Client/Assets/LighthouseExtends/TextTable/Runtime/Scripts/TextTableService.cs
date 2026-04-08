using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using VContainer;

namespace LighthouseExtends.TextTable
{
    public sealed class TextTableService : ITextTableService, IDisposable
    {
        public static ITextTableService Instance { get; private set; }

        readonly ITextTableLoader loader;
        readonly ReactiveProperty<string> currentLanguage;

        IReadOnlyDictionary<string, string> activeTable;

        public ReadOnlyReactiveProperty<string> CurrentLanguage => currentLanguage;

        [Inject]
        public TextTableService(ITextTableLoader loader)
        {
            Instance = this;
            this.loader = loader;
            currentLanguage = new ReactiveProperty<string>(string.Empty);
        }

        public string GetText(ITextData textData)
        {
            if (activeTable == null || !activeTable.TryGetValue(textData.TextKey, out var text))
            {
                return textData.TextKey;
            }

            if (textData.Params != null && textData.Params.Count > 0)
            {
                text = FormatText(text, textData.Params);
            }

            return text;
        }

        public async UniTask SetLanguage(string languageCode, CancellationToken cancellationToken)
        {
            activeTable = await loader.LoadAsync(languageCode, cancellationToken);
            currentLanguage.Value = languageCode;
        }

        public void Dispose()
        {
            currentLanguage.Dispose();
            Instance = null;
        }

        static string FormatText(string text, IReadOnlyDictionary<string, object> textParams)
        {
            foreach (var kvp in textParams)
            {
                text = text.Replace($"{{{kvp.Key}}}", kvp.Value?.ToString() ?? string.Empty);
            }

            return text;
        }
    }
}
