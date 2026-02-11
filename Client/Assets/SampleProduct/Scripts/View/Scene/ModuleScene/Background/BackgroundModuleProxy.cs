using System;
using Cysharp.Threading.Tasks;

namespace SampleProduct.View.Scene.ModuleScene.Background
{
    public class BackgroundModuleProxy : IBackgroundModule, IBackgroundModuleProxy
    {
        IBackgroundModuleImpl module;

        async UniTask IBackgroundModule.SetBackground(string backgroundAsset)
        {
            if (module != null)
            {
                await module.SetBackground(backgroundAsset);
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