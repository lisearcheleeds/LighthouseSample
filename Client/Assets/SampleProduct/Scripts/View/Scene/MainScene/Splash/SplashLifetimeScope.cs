using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace SampleProduct.View.Scene.MainScene.Splash
{
    public class SplashLifetimeScope : LifetimeScope
    {
        [SerializeField] SplashScene splashScene;
        [SerializeField] SplashView splashView;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(splashScene);

            builder.Register<SplashPresenter>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterComponent(splashView).AsImplementedInterfaces();
        }
    }
}