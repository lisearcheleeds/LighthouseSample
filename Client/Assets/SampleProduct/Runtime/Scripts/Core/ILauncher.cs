using Cysharp.Threading.Tasks;

namespace SampleProduct.Core
{
    public interface ILauncher
    {
        UniTask Launch();
        void Reboot();
    }
}