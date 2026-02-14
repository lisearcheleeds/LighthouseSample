using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
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
        readonly IModuleSceneManager moduleSceneManager;

        [Inject]
        public ProductEntryPoint(
            ProductLifetimeScope productLifetimeScope,
            ProductLifetimeScopeSettings productLifetimeScopeSettings,
            ILauncher launcher,
            IMainSceneManager mainSceneManager,
            IModuleSceneManager moduleSceneManager)
        {
            this.productLifetimeScope = productLifetimeScope;
            this.productLifetimeScopeSettings = productLifetimeScopeSettings;
            this.launcher = launcher;
            this.mainSceneManager = mainSceneManager;
            this.moduleSceneManager = moduleSceneManager;
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            mainSceneManager.SetEnqueueParentLifetimeScope(() => LifetimeScope.EnqueueParent(productLifetimeScope));
            moduleSceneManager.SetEnqueueParentLifetimeScope(() => LifetimeScope.EnqueueParent(productLifetimeScope));

            await launcher.Launch();
        }
    }
}