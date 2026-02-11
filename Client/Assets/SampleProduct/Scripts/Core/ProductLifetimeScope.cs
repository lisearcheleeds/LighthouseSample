using Lighthouse.Core.Scene;
using Lighthouse.Core.Scene.SceneCamera;
using Lighthouse.Extends.CanvasSceneObject;
using Lighthouse.Extends.InputBlocker;
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
        [SerializeField] LHCanvasSceneObject canvasSceneObjectPrefab;
        [SerializeField] LHInputBlocker inputBlockerPrefab;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<ProductEntryPoint>();
            builder.RegisterInstance(productLifetimeScopeSettings);

            builder.Register<Launcher>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.Register<SceneManager>(Lifetime.Singleton);
            builder.Register<SceneGroupController>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<SceneGroupProvider>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<MainSceneManager>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<ModuleSceneManager>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<SceneCameraManager>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.RegisterComponentInNewPrefab(canvasSceneObjectPrefab, Lifetime.Singleton).DontDestroyOnLoad().AsImplementedInterfaces();
            builder.RegisterComponentInNewPrefab(inputBlockerPrefab, Lifetime.Singleton).DontDestroyOnLoad().AsImplementedInterfaces();

            builder.Register<OverlayModuleProxy>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<GlobalHeaderModuleProxy>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<BackgroundModuleProxy>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}