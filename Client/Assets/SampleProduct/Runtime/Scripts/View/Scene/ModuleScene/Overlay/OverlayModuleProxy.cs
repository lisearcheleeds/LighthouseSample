using System;
using Cysharp.Threading.Tasks;

namespace SampleProduct.View.Scene.ModuleScene.Overlay
{
    public sealed class OverlayModuleProxy : IOverlayModule, IOverlayModuleProxy
    {
        IOverlayModuleImpl module;

        UniTask IOverlayModule.PlayInAnimation()
        {
            if (module != null)
            {
                return module.PlayInAnimation();
            }

            return UniTask.CompletedTask;
        }

        UniTask IOverlayModule.PlayOutAnimation()
        {
            if (module != null)
            {
                return module.PlayOutAnimation();
            }

            return UniTask.CompletedTask;
        }

        void IOverlayModuleProxy.RegisterModule(IOverlayModuleImpl module)
        {
            if (this.module != null)
            {
                throw new ArgumentException($"Duplicate register");
            }

            this.module = module;
        }

        void IOverlayModuleProxy.UnregisterModule(IOverlayModuleImpl module)
        {
            if (this.module != module)
            {
                throw new ArgumentException($"No match impl");
            }

            this.module = null;
        }
    }
}