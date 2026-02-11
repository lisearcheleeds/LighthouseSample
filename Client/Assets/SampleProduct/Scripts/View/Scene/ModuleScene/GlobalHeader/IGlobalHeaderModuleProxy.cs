namespace SampleProduct.View.Scene.ModuleScene.GlobalHeader
{
    public interface IGlobalHeaderModuleProxy
    {
        void RegisterModule(IGlobalHeaderModuleImpl module);
        void UnregisterModule(IGlobalHeaderModuleImpl module);
    }
}