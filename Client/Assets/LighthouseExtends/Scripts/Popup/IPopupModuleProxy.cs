namespace LighthouseExtends.Popup
{
    public interface IPopupModuleProxy
    {
        void RegisterModule(IPopupModuleImpl module);
        void UnregisterModule(IPopupModuleImpl module);
    }
}