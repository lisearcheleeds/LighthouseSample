using System.Collections.Generic;
using LighthouseExtends.Language;
using VContainer;

namespace SampleProduct.Infrastructure.Language
{
    public sealed class SupportedLanguageService : ISupportedLanguageService
    {
        readonly SupportedLanguageSettings settings;

        [Inject]
        public SupportedLanguageService(SupportedLanguageSettings settings)
        {
            this.settings = settings;
        }

        public IReadOnlyList<string> SupportedLanguages => settings.SupportedLanguages;
        public string DefaultLanguage => settings.DefaultLanguage;
    }
}
