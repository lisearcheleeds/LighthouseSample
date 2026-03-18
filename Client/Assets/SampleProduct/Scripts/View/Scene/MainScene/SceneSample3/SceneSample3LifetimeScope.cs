using LighthouseExtends.UIComponent.Scripts.Button;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace SampleProduct.View.Scene.MainScene.SceneSample3
{
    public class SceneSample3LifetimeScope : LifetimeScope
    {
        [SerializeField] SceneSample3Scene sceneSample3Scene;
        [SerializeField] SceneSample3View sceneSample3View;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(sceneSample3Scene);
            builder.RegisterComponentInHierarchy<LHButton>();

            builder.Register<SceneSample3Presenter>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterComponent(sceneSample3View).AsImplementedInterfaces();
        }
    }
}