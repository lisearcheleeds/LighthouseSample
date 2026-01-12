using Lighthouse.Core.Scene;
using Product.Util;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Product
{
    public class ProductLifetimeScope : LifetimeScope
    {
        [SerializeField] ProductLifetimeScopeSettings productLifetimeScopeSettings;
        [SerializeField] CanvasSceneObject canvasSceneObjectPrefab;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<ProductEntryPoint>();
            builder.RegisterInstance(productLifetimeScopeSettings);

            builder.Register<Launcher>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<SceneManager>(Lifetime.Singleton);
            builder.Register<SceneGroupController>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<MainSceneGroupProvider>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<CommonSceneManager>(Lifetime.Singleton);
            builder.Register<SceneCameraManager>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.RegisterComponentInNewPrefab(canvasSceneObjectPrefab, Lifetime.Singleton).DontDestroyOnLoad().AsImplementedInterfaces();
        }
    }
}