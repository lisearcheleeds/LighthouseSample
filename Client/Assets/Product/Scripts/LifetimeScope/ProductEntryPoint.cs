using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Core.Scene;
using Product.Util;
using VContainer;
using VContainer.Unity;

namespace Product.LifetimeScope
{
    public class ProductEntryPoint : IAsyncStartable
    {
        readonly ProductLifetimeScope productLifetimeScope;
        readonly ProductLifetimeScopeSettings productLifetimeScopeSettings;
        readonly ILauncher launcher;
        readonly IMainSceneGroupProvider mainSceneGroupProvider;

        [Inject]
        public ProductEntryPoint(ProductLifetimeScope productLifetimeScope, ProductLifetimeScopeSettings productLifetimeScopeSettings, ILauncher launcher, IMainSceneGroupProvider mainSceneGroupProvider)
        {
            this.productLifetimeScope = productLifetimeScope;
            this.productLifetimeScopeSettings = productLifetimeScopeSettings;
            this.launcher = launcher;
            this.mainSceneGroupProvider = mainSceneGroupProvider;
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            mainSceneGroupProvider.SetEnqueueParentLifetimeScope(() => VContainer.Unity.LifetimeScope.EnqueueParent(productLifetimeScope));

            await launcher.Launch();
        }
    }
}