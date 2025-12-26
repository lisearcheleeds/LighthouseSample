using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace SampleProduct.View.Scene.MainScene.SampleTop
{
    public class SampleTopLifetimeScope : LifetimeScope
    {
        [SerializeField] SampleTopScene sampleTopScene;
        [SerializeField] SampleTopView sampleTopView;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(sampleTopScene);

            builder.Register<SampleTopPresenter>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterComponent(sampleTopView).AsImplementedInterfaces();
        }
    }
}