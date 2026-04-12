using System.Collections.Generic;

namespace LighthouseExtends.Language
{
    public interface ISupportedLanguageService
    {
        IReadOnlyList<string> SupportedLanguages { get; }
        string DefaultLanguage { get; }
    }
}
