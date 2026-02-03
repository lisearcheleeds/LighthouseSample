using Lighthouse.Core.Scene;
using Lighthouse.Core.Scene.SceneCamera;
using Lighthouse.Extends.CanvasSceneObject;
using Lighthouse.Extends.InputBlocker;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Product.Core
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
            builder.Register<MainSceneGroupProvider>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<CommonSceneManager>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<SceneCameraManager>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.RegisterComponentInNewPrefab(canvasSceneObjectPrefab, Lifetime.Singleton).DontDestroyOnLoad().AsImplementedInterfaces();
            builder.RegisterComponentInNewPrefab(inputBlockerPrefab, Lifetime.Singleton).DontDestroyOnLoad().AsImplementedInterfaces();
        }
    }
}