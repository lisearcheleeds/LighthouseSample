using System;
using VContainer;
using VContainer.Unity;

namespace SampleProduct.View.Scene.ModuleScene.Overlay
{
    public sealed class OverlayEntryPoint : IStartable, IDisposable
    {
        readonly IOverlayModuleProxy overlayModuleProxy;
        readonly IOverlayModuleImpl overlayModule;

        [Inject]
        public OverlayEntryPoint(IOverlayModuleProxy overlayModuleProxy, IOverlayModuleImpl overlayModule)
        {
            this.overlayModuleProxy = overlayModuleProxy;
            this.overlayModule = overlayModule;
        }

        void IStartable.Start()
        {
            overlayModuleProxy.RegisterModule(overlayModule);
        }

        void IDisposable.Dispose()
        {
            overlayModuleProxy.UnregisterModule(overlayModule);
        }
    }
}