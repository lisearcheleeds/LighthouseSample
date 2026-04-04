using System;

namespace SampleProduct.View.Scene.ModuleScene.Background
{
    public class BackgroundModuleProxy : IBackgroundModule, IBackgroundModuleProxy
    {
        IBackgroundModuleImpl module;

        void IBackgroundModule.SetBackgroundLayout(BackgroundLayout backgroundLayout)
        {
            if (module != null)
            {
                module.SetBackgroundLayout(backgroundLayout);
            }
        }

        void IBackgroundModuleProxy.RegisterModule(IBackgroundModuleImpl module)
        {
            if (this.module != null)
            {
                throw new ArgumentException($"Duplicate register");
            }

            this.module = module;
        }

        void IBackgroundModuleProxy.UnregisterModule(IBackgroundModuleImpl module)
        {
            if (this.module != module)
            {
                throw new ArgumentException($"No match impl");
            }

            this.module = null;
        }
    }
}