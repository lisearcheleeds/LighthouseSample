using R3;
using TMPro;

namespace LighthouseExtends.Font
{
    public interface IFontService
    {
        ReadOnlyReactiveProperty<TMP_FontAsset> CurrentFont { get; }
        TMP_FontAsset GetFont(string languageCode);
    }
}
