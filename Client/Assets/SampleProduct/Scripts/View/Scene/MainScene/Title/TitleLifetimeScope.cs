using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace SampleProduct.View.Scene.MainScene.Title
{
    public class TitleLifetimeScope : LifetimeScope
    {
        [SerializeField] TitleScene titleScene;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(titleScene);
        }
    }
}