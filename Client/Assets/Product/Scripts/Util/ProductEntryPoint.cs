using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

namespace Product.Util
{
    public class ProductEntryPoint : IAsyncStartable
    {
        readonly ILauncher launcher;

        [Inject]
        public ProductEntryPoint(ILauncher launcher)
        {
            this.launcher = launcher;
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            await launcher.Launch();
        }
    }
}