using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using LighthouseExtends.Language;
using VContainer;

namespace LighthouseExtends.TextTable
{
    public sealed class TextTableService : ITextTableService, IDisposable
    {
        public static ITextTableService Instance { get; private set; }

        readonly ITextTableLoader loader;

        IReadOnlyDictionary<string, string> activeTable;

        [Inject]
        public TextTableService(ITextTableLoader loader, ILanguageService languageService)
        {
            Instance = this;
            this.loader = loader;
            languageService.RegisterChangeHandler(LoadTableAsync);
        }

        public string GetText(ITextData textData)
        {
            if (activeTable == null || !activeTable.TryGetValue(textData.TextKey, out var text))
            {
                return textData.TextKey;
            }

            if (textData.Params != null && 0 < textData.Params.Count)
            {
                text = FormatText(text, textData.Params);
            }

            return text;
        }

        public void Dispose()
        {
            Instance = null;
        }

        async UniTask LoadTableAsync(string languageCode, CancellationToken cancellationToken)
        {
            activeTable = await loader.LoadAsync(languageCode, cancellationToken);
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
