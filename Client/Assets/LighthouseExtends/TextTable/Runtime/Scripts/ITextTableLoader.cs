using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace LighthouseExtends.TextTable
{
    public interface ITextTableLoader
    {
        UniTask<IReadOnlyDictionary<string, string>> LoadAsync(string languageCode, CancellationToken cancellationToken);
    }
}
