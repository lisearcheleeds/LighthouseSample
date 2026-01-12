using Cysharp.Threading.Tasks;

namespace Product.Util
{
    public interface ILauncher
    {
        UniTask Launch();
        void Reboot();
    }
}