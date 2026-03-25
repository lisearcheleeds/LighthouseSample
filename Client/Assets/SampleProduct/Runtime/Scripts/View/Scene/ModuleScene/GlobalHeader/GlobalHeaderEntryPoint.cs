using System;
using VContainer;
using VContainer.Unity;

namespace SampleProduct.View.Scene.ModuleScene.GlobalHeader
{
    public sealed class GlobalHeaderEntryPoint : IStartable, IDisposable
    {
        readonly IGlobalHeaderModuleProxy globalHeaderModuleProxy;
        readonly IGlobalHeaderModuleImpl globalHeaderModule;

        [Inject]
        public GlobalHeaderEntryPoint(IGlobalHeaderModuleProxy globalHeaderModuleProxy, IGlobalHeaderModuleImpl globalHeaderModule)
        {
            this.globalHeaderModuleProxy = globalHeaderModuleProxy;
            this.globalHeaderModule = globalHeaderModule;
        }

        void IStartable.Start()
        {
            globalHeaderModuleProxy.RegisterModule(globalHeaderModule);
        }

        void IDisposable.Dispose()
        {
            globalHeaderModuleProxy.UnregisterModule(globalHeaderModule);
        }
    }
}