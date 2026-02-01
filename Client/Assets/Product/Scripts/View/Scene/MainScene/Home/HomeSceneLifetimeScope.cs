using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Product.View.Scene.MainScene.Home
{
    public class HomeSceneLifetimeScope : VContainer.Unity.LifetimeScope
    {
        [SerializeField] HomeScene homeScene;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(homeScene);
        }
    }
}