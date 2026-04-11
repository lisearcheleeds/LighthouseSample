using Lighthouse.Scene;
using Lighthouse.Scene.SceneCamera;
using LighthouseExtends.Font;
using LighthouseExtends.Language;
using LighthouseExtends.ScreenStack;
using LighthouseExtends.TextTable;
using LighthouseExtends.UIComponent.CanvasSceneObject;
using LighthouseExtends.UIComponent.ExclusiveInput;
using LighthouseExtends.UIComponent.InputBlocker;
using SampleProduct.Infrastructure.AssetLoader;
using SampleProduct.View.Scene.ModuleScene.Background;
using SampleProduct.View.Scene.ModuleScene.GlobalHeader;
using SampleProduct.View.Scene.ModuleScene.Overlay;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace SampleProduct.Core
{
    public class ProductLifetimeScope : LifetimeScope
    {
        [SerializeField] ProductLifetimeScopeSettings productLifetimeScopeSettings;
        [SerializeField] LanguageFontSettings languageFontSettings;
        [SerializeField] LHCanvasSceneObject canvasSceneObjectPrefab;
        [SerializeField] LHInputBlocker inputBlockerPrefab;

        protected override void Configure(IContainerBuilder builder)
        {
            // Product
            builder.RegisterEntryPoint<ProductEntryPoint>();
            builder.RegisterInstance(productLifetimeScopeSettings);

            {
                // LightHouse
                builder.Register<SceneManager>(Lifetime.Singleton).AsImplementedInterfaces();
                builder.Register<SceneTransitionController>(Lifetime.Singleton).AsImplementedInterfaces();
                builder.Register<SceneGroupProvider>(Lifetime.Singleton).AsImplementedInterfaces();
                builder.Register<MainSceneManager>(Lifetime.Singleton).AsImplementedInterfaces();
                builder.Register<ModuleSceneManager>(Lifetime.Singleton).AsImplementedInterfaces();
                builder.Register<SceneCameraManager>(Lifetime.Singleton).AsImplementedInterfaces();

                builder.Register<DefaultSceneTransitionSequenceProvider>(Lifetime.Singleton).AsImplementedInterfaces();
            }

            {
                // LightHouse.Extends
                builder.Register<ExclusiveInputService>(Lifetime.Singleton).AsImplementedInterfaces();

                builder.Register<LanguageService>(Lifetime.Singleton).AsImplementedInterfaces();
                builder.Register<TextTableService>(Lifetime.Singleton).AsImplementedInterfaces();
                builder.RegisterInstance(languageFontSettings);
                builder.Register<FontService>(Lifetime.Singleton).AsImplementedInterfaces();

                // Modules
                builder.Register<ScreenStackModuleProxy>(Lifetime.Singleton).AsImplementedInterfaces();
            }

            {
                // SampleProduct
                builder.Register<Launcher>(Lifetime.Singleton).AsImplementedInterfaces();

                {
                    // LightHouse Require
                    builder.RegisterComponentInNewPrefab(canvasSceneObjectPrefab, Lifetime.Singleton).DontDestroyOnLoad().AsImplementedInterfaces();
                    builder.RegisterComponentInNewPrefab(inputBlockerPrefab, Lifetime.Singleton).DontDestroyOnLoad().AsImplementedInterfaces();
                }

                builder.Register<SampleAssetLoader>(Lifetime.Singleton).AsImplementedInterfaces();

                // Module
                builder.Register<OverlayModuleProxy>(Lifetime.Singleton).AsImplementedInterfaces();
                builder.Register<GlobalHeaderModuleProxy>(Lifetime.Singleton).AsImplementedInterfaces();
                builder.Register<BackgroundModuleProxy>(Lifetime.Singleton).AsImplementedInterfaces();
            }
        }

    }
}
