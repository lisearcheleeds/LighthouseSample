using VContainer;

namespace LighthouseExtends.Popup
{
    public class PopupModule : IPopupModuleImpl
    {
        readonly PopupModuleScene popupModuleScene;

        [Inject]
        public PopupModule(PopupModuleScene popupModuleScene)
        {
            this.popupModuleScene = popupModuleScene;
        }
    }
}