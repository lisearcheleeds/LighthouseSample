using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace SampleProduct.View.Scene.MainScene.Home
{
    public class HomeSceneLifetimeScope : LifetimeScope
    {
        [SerializeField] HomeScene homeScene;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(homeScene);
        }
    }
}