using System;

namespace SampleProduct.View.Scene.ModuleScene.GlobalHeader
{
    public sealed class GlobalHeaderModuleProxy : IGlobalHeaderModule, IGlobalHeaderModuleProxy
    {
        IGlobalHeaderModuleImpl module;

        void IGlobalHeaderModule.SetHeaderText(string headerText)
        {
            if (module != null)
            {
                module.SetHeaderText(headerText);
            }
        }

        void IGlobalHeaderModuleProxy.RegisterModule(IGlobalHeaderModuleImpl module)
        {
            if (this.module != null)
            {
                throw new ArgumentException($"Duplicate register");
            }

            this.module = module;
        }

        void IGlobalHeaderModuleProxy.UnregisterModule(IGlobalHeaderModuleImpl module)
        {
            if (this.module != module)
            {
                throw new ArgumentException($"No match impl");
            }

            this.module = null;
        }
    }
}