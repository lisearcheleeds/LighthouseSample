using LighthouseExtends.UIComponent.Scripts.Button;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace SampleProduct.View.Scene.MainScene.SceneSample1
{
    public class SceneSample1LifetimeScope : LifetimeScope
    {
        [SerializeField] SceneSample1Scene sceneSample1Scene;
        [SerializeField] SceneSample1View sceneSample1View;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(sceneSample1Scene);
            builder.RegisterComponentInHierarchy<LHButton>();

            builder.Register<SceneSample1Presenter>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterComponent(sceneSample1View).AsImplementedInterfaces();
        }
    }
}