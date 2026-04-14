using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;

namespace LighthouseExtends.Language
{
    public interface ILanguageService
    {
        ReadOnlyReactiveProperty<string> CurrentLanguage { get; }
        void RegisterChangeHandler(Func<string, CancellationToken, UniTask> handler);
        UniTask SetLanguage(string languageCode, CancellationToken cancellationToken);
    }
}
