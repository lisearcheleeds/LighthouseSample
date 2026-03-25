using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace LighthouseExtends.ScreenStack
{
    /// <summary>
    /// You need to register the following on this LifetimeScope:
    /// IPopupPresenterFactory
    /// IPopupFactory
    /// </summary>
    public abstract class ScreenStackLifetimeScopeBase : LifetimeScope
    {
        [SerializeField] ScreenStackModuleSceneBase screenStackModuleSceneBase;
        [SerializeField] ScreenStackCanvasController screenStackCanvasController;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<ScreenStackEntryPoint>();
            builder.RegisterComponent(screenStackModuleSceneBase);
            builder.RegisterComponent(screenStackCanvasController).AsImplementedInterfaces();

            builder.Register<ScreenStackModule>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<ScreenStackManager>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}