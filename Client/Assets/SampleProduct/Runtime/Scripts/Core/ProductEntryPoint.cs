using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
using LighthouseExtends.Language;
using UnityEngine;
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
        readonly ILanguageService languageService;

        [Inject]
        public ProductEntryPoint(
            ProductLifetimeScope productLifetimeScope,
            ProductLifetimeScopeSettings productLifetimeScopeSettings,
            ILauncher launcher,
            IMainSceneManager mainSceneManager,
            IModuleSceneManager moduleSceneManager,
            ILanguageService languageService)
        {
            this.productLifetimeScope = productLifetimeScope;
            this.productLifetimeScopeSettings = productLifetimeScopeSettings;
            this.launcher = launcher;
            this.mainSceneManager = mainSceneManager;
            this.moduleSceneManager = moduleSceneManager;
            this.languageService = languageService;
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            mainSceneManager.SetEnqueueParentLifetimeScope(() => LifetimeScope.EnqueueParent(productLifetimeScope));
            moduleSceneManager.SetEnqueueParentLifetimeScope(() => LifetimeScope.EnqueueParent(productLifetimeScope));

            await languageService.SetLanguage(ToLanguageCode(Application.systemLanguage), cancellation);
            await launcher.Launch();
        }

        static string ToLanguageCode(SystemLanguage language)
        {
            return language switch
            {
                SystemLanguage.Japanese => "ja",
                _ => "en",
            };
        }
    }
}
