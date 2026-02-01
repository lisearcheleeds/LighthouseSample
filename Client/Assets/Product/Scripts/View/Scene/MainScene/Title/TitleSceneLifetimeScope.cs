using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Product.View.Scene.MainScene.Title
{
    public class TitleSceneLifetimeScope : VContainer.Unity.LifetimeScope
    {
        [SerializeField] TitleScene titleScene;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(titleScene);
        }
    }
}