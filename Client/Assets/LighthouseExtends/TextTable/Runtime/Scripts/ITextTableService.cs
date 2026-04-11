using R3;

namespace LighthouseExtends.TextTable
{
    public interface ITextTableService
    {
        ReadOnlyReactiveProperty<string> CurrentLanguage { get; }
        string GetText(ITextData textData);
    }
}
