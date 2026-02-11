using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace SampleProduct.View.Scene.SceneModule.GlobalHeader
{
    public class GlobalHeaderLifetimeScope : LifetimeScope
    {
        [SerializeField] GlobalHeaderSceneModule globalHeaderSceneModule;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<GlobalHeaderEntryPoint>();
            builder.RegisterComponent(globalHeaderSceneModule);
        }
    }
}