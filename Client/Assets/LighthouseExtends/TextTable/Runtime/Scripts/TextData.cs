using System.Collections.Generic;

namespace LighthouseExtends.TextTable
{
    public class TextData : ITextData
    {
        public string TextKey { get; }
        public IReadOnlyDictionary<string, object> Params { get; }

        public TextData(string textKey, IReadOnlyDictionary<string, object> textParams = null)
        {
            TextKey = textKey;
            Params = textParams;
        }
    }
}
