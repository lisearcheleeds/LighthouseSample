using Lighthouse.Scene;
using Lighthouse.Scene.SceneBase;

namespace LighthouseExtends.Popup
{
    public class PopupModuleScene : CanvasModuleSceneBase
    {
        public override ModuleSceneId ModuleSceneId => PopupModuleSceneId.Popup;

        public override bool IsAlwaysInAnimation => true;
        public override bool IsAlwaysOutAnimation => true;
    }
}