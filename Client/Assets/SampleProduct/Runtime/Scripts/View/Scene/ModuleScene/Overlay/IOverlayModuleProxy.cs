namespace SampleProduct.View.Scene.ModuleScene.Overlay
{
    public interface IOverlayModuleProxy
    {
        void RegisterModule(IOverlayModuleImpl module);
        void UnregisterModule(IOverlayModuleImpl module);
    }
}