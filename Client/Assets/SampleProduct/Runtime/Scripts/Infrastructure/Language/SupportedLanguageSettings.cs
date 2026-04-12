using System.Collections.Generic;
using UnityEngine;

namespace SampleProduct.Infrastructure.Language
{
    [CreateAssetMenu(
        fileName = "SupportedLanguageSettings",
        menuName = "SampleProduct/SupportedLanguageSettings")]
    public class SupportedLanguageSettings : ScriptableObject
    {
        [SerializeField] List<string> supportedLanguages = new() { "en", "ja", "zh", "ko" };
        [SerializeField] string defaultLanguage = "en";

        public IReadOnlyList<string> SupportedLanguages => supportedLanguages;
        public string DefaultLanguage => defaultLanguage;
    }
}
