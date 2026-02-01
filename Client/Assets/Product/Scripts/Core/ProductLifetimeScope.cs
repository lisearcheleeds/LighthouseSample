using Lighthouse.Core.Scene;
using Lighthouse.Core.Scene.SceneCamera;
using Lighthouse.Extends.CanvasSceneObject;
using Lighthouse.Extends.InputBlocker;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;
using VContainer.Unity;

namespace Product.Core
{
    public class ProductLifetimeScope : LifetimeScope
    {
        [SerializeField] ProductLifetimeScopeSettings productLifetimeScopeSettings;
        [FormerlySerializedAs("canvasSceneObjectPrefab")] [SerializeField] LHCanvasSceneObject lhCanvasSceneObjectPrefab;
        [FormerlySerializedAs("inputBlockerPrefab")] [SerializeField] LHInputBlocker lhInputBlockerPrefab;

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

            builder.RegisterComponentInNewPrefab(lhCanvasSceneObjectPrefab, Lifetime.Singleton).DontDestroyOnLoad().AsImplementedInterfaces();
            builder.RegisterComponentInNewPrefab(lhInputBlockerPrefab, Lifetime.Singleton).DontDestroyOnLoad().AsImplementedInterfaces();
        }
    }
}