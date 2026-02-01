using Cysharp.Threading.Tasks;

namespace Product.Core
{
    public interface ILauncher
    {
        UniTask Launch();
        void Reboot();
    }
}