namespace SampleProduct.View.Scene.ModuleScene.Background
{
    public interface IBackgroundModuleProxy
    {
        void RegisterModule(IBackgroundModuleImpl module);
        void UnregisterModule(IBackgroundModuleImpl module);
    }
}