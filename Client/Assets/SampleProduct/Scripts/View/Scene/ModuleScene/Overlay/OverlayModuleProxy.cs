using System;
using Cysharp.Threading.Tasks;

namespace SampleProduct.View.Scene.ModuleScene.Overlay
{
    public sealed class OverlayModuleProxy : IOverlayModule, IOverlayModuleProxy
    {
        IOverlayModuleImpl module;

        UniTask IOverlayModule.PlayInAnimation(bool withStateChange)
        {
            if (module != null)
            {
                return module.PlayInAnimation(withStateChange);
            }

            return UniTask.CompletedTask;
        }

        UniTask IOverlayModule.PlayOutAnimation(bool withStateChange)
        {
            if (module != null)
            {
                return module.PlayOutAnimation(withStateChange);
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