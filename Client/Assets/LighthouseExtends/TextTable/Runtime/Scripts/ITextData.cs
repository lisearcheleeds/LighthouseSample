using System.Collections.Generic;

namespace LighthouseExtends.TextTable
{
    public interface ITextData
    {
        string TextKey { get; }
        IReadOnlyDictionary<string, object> Params { get; }
    }
}
