using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace SampleProduct.View.Scene.MainScene.Purpose
{
    public class PurposeLifetimeScope : LifetimeScope
    {
        [SerializeField] PurposeScene purposeScene;
        [SerializeField] PurposeView purposeView;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(purposeScene);

            builder.Register<PurposePresenter>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterComponent(purposeView).AsImplementedInterfaces();
        }
    }
}
