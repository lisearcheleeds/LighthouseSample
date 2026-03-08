using LighthouseExtends.UIComponent.Scripts.Button;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace SampleProduct.View.Scene.MainScene.SceneSample2
{
    public class SceneSample2LifetimeScope : LifetimeScope
    {
        [SerializeField] SceneSample2Scene sceneSample2Scene;
        [SerializeField] SceneSample2View sceneSample2View;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(sceneSample2Scene);
            builder.RegisterComponentInHierarchy<LHButton>();

            builder.Register<SceneSample2Presenter>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterComponent(sceneSample2View).AsImplementedInterfaces();
        }
    }
}