using Product.View.Splash;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace ProductNameSpace
{
    public class SplashSceneLifetimeScope : LifetimeScope
    {
        [SerializeField] SplashScene splashScene;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(splashScene);
        }
    }
}