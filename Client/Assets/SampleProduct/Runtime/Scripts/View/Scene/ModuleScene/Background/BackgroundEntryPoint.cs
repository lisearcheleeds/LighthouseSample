using System;
using VContainer;
using VContainer.Unity;

namespace SampleProduct.View.Scene.ModuleScene.Background
{
    public sealed class BackgroundEntryPoint : IStartable, IDisposable
    {
        readonly IBackgroundModuleProxy backgroundModuleProxy;
        readonly IBackgroundModuleImpl backgroundModuleImpl;

        [Inject]
        public BackgroundEntryPoint(IBackgroundModuleProxy backgroundModuleProxy, IBackgroundModuleImpl backgroundModuleImpl)
        {
            this.backgroundModuleProxy = backgroundModuleProxy;
            this.backgroundModuleImpl = backgroundModuleImpl;
        }

        void IStartable.Start()
        {
            backgroundModuleProxy.RegisterModule(backgroundModuleImpl);
        }

        void IDisposable.Dispose()
        {
            backgroundModuleProxy.UnregisterModule(backgroundModuleImpl);
        }
    }
}