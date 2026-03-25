using LighthouseExtends.UIComponent.Button;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace SampleProduct.View.Scene.MainScene.Title
{
    public class TitleLifetimeScope : LifetimeScope
    {
        [SerializeField] TitleScene titleScene;
        [SerializeField] TitleView titleView;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(titleScene);
            builder.RegisterComponentInHierarchy<LHButton>();

            builder.Register<TitlePresenter>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterComponent(titleView).AsImplementedInterfaces();
        }
    }
}