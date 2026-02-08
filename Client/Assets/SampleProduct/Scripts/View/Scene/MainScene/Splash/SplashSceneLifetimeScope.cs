using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace SampleProduct.View.Scene.MainScene.Splash
{
    public class SplashSceneLifetimeScope : VContainer.Unity.LifetimeScope
    {
        [SerializeField] SplashScene splashScene;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(splashScene);
        }
    }
}