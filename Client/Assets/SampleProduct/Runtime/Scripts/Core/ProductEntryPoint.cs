using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
using LighthouseExtends.Addressable;
using LighthouseExtends.Language;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace SampleProduct.Core
{
    public class ProductEntryPoint : IAsyncStartable, IDisposable
    {
        static readonly IReadOnlyList<string> PreloadAddresses = new[]
        {
            "AnimationElementOverlay",
            "ButtonElementOverlay",
            "DialogElementDialog",
            "DialogSample1Dialog",
            "DialogSample2Dialog",
            "DialogSampleConfirmDialog",
            "InputLayerElementOverlay",
            "OverlayElementOverlay",
            "PopupElementPopup",
            "RequireToolsDialog",
            "SceneTransitionDialog",
            "TextViewElementOverlay",
            "TransitionAnimationElementOverlay",
        };

        readonly ProductLifetimeScope productLifetimeScope;
        readonly ProductLifetimeScopeSettings productLifetimeScopeSettings;
        readonly ILauncher launcher;
        readonly IMainSceneManager mainSceneManager;
        readonly IModuleSceneManager moduleSceneManager;
        readonly ILanguageService languageService;
        readonly ISupportedLanguageService supportedLanguageService;
        readonly IAssetManager assetManager;

        IAssetScope preloadScope;

        [Inject]
        public ProductEntryPoint(
            ProductLifetimeScope productLifetimeScope,
            ProductLifetimeScopeSettings productLifetimeScopeSettings,
            ILauncher launcher,
            IMainSceneManager mainSceneManager,
            IModuleSceneManager moduleSceneManager,
            ILanguageService languageService,
            ISupportedLanguageService supportedLanguageService,
            IAssetManager assetManager)
        {
            this.productLifetimeScope = productLifetimeScope;
            this.productLifetimeScopeSettings = productLifetimeScopeSettings;
            this.launcher = launcher;
            this.mainSceneManager = mainSceneManager;
            this.moduleSceneManager = moduleSceneManager;
            this.languageService = languageService;
            this.supportedLanguageService = supportedLanguageService;
            this.assetManager = assetManager;
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            mainSceneManager.SetEnqueueParentLifetimeScope(() => LifetimeScope.EnqueueParent(productLifetimeScope));
            moduleSceneManager.SetEnqueueParentLifetimeScope(() => LifetimeScope.EnqueueParent(productLifetimeScope));

            await languageService.SetLanguage(ResolveInitialLanguage(Application.systemLanguage), cancellation);

            preloadScope = assetManager.CreateScope();
            await preloadScope.LoadAsync<GameObject>(PreloadAddresses, cancellation);

            await launcher.Launch();
        }

        public void Dispose()
        {
            preloadScope?.Dispose();
        }

        string ResolveInitialLanguage(SystemLanguage systemLanguage)
        {
            var code = systemLanguage switch
            {
                SystemLanguage.Japanese => "ja",
                SystemLanguage.ChineseSimplified => "zh",
                SystemLanguage.ChineseTraditional => "zh",
                SystemLanguage.Korean => "ko",
                _ => "en",
            };

            var supported = supportedLanguageService.SupportedLanguages;
            for (var i = 0; i < supported.Count; i++)
            {
                if (supported[i] == code) { return code; }
            }

            return supportedLanguageService.DefaultLanguage;
        }
    }
}

