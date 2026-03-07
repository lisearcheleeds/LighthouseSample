using LighthouseExtends.UIComponent.Scripts.Button;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace SampleProduct.View.Scene.MainScene.Home
{
    public class HomeLifetimeScope : LifetimeScope
    {
        [SerializeField] HomeScene homeScene;
        [SerializeField] HomeView homeView;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(homeScene);
            builder.RegisterComponentInHierarchy<LHButton>();

            builder.Register<HomeViewController>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterComponent(homeView).AsImplementedInterfaces();
        }
    }
}