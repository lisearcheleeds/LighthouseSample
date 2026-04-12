using System.Collections.Generic;
using UnityEngine;

namespace LighthouseExtends.Language
{
    [CreateAssetMenu(menuName = "Lighthouse/Language/Supported Language Settings")]
    public class SupportedLanguageSettings : ScriptableObject
    {
        [SerializeField] List<string> supportedLanguages = new() { "en" };
        [SerializeField] string defaultLanguage = "en";

        public IReadOnlyList<string> SupportedLanguages => supportedLanguages;
        public string DefaultLanguage => defaultLanguage;
    }
}
