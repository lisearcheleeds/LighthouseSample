using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using VContainer;

namespace LighthouseExtends.Language
{
    public sealed class LanguageService : ILanguageService, IDisposable
    {
        public static ILanguageService Instance { get; private set; }

        readonly ReactiveProperty<string> currentLanguage = new(string.Empty);
        readonly List<Func<string, CancellationToken, UniTask>> changeHandlers = new();

        public ReadOnlyReactiveProperty<string> CurrentLanguage => currentLanguage;

        [Inject]
        public LanguageService()
        {
            Instance = this;
        }

        public void RegisterChangeHandler(Func<string, CancellationToken, UniTask> handler)
        {
            changeHandlers.Add(handler);
        }

        public async UniTask SetLanguage(string languageCode, CancellationToken cancellationToken)
        {
            await UniTask.WhenAll(changeHandlers.Select(h => h(languageCode, cancellationToken)));
            currentLanguage.Value = languageCode;
        }

        public void Dispose()
        {
            currentLanguage.Dispose();
            Instance = null;
        }
    }
}
