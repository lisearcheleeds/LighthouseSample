using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Core.Scene;
using VContainer;
using VContainer.Unity;

namespace SampleProduct.Core
{
    public class ProductEntryPoint : IAsyncStartable
    {
        readonly ProductLifetimeScope productLifetimeScope;
        readonly ProductLifetimeScopeSettings productLifetimeScopeSettings;
        readonly ILauncher launcher;
        readonly IMainSceneManager mainSceneManager;

        [Inject]
        public ProductEntryPoint(
            ProductLifetimeScope productLifetimeScope,
            ProductLifetimeScopeSettings productLifetimeScopeSettings,
            ILauncher launcher,
            IMainSceneManager mainSceneManager)
        {
            this.productLifetimeScope = productLifetimeScope;
            this.productLifetimeScopeSettings = productLifetimeScopeSettings;
            this.launcher = launcher;
            this.mainSceneManager = mainSceneManager;
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            mainSceneManager.SetEnqueueParentLifetimeScope(() => LifetimeScope.EnqueueParent(productLifetimeScope));

            await launcher.Launch();
        }
    }
}