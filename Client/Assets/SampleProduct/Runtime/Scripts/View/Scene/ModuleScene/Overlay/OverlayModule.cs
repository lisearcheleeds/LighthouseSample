using Cysharp.Threading.Tasks;
using VContainer;

namespace SampleProduct.View.Scene.ModuleScene.Overlay
{
    public class OverlayModule : IOverlayModuleImpl
    {
        readonly OverlayModuleScene overlayModuleScene;

        [Inject]
        public OverlayModule(OverlayModuleScene overlayModuleScene)
        {
            this.overlayModuleScene = overlayModuleScene;
        }

        UniTask IOverlayModule.PlayInAnimation()
        {
            return overlayModuleScene.PlayInAnimation();
        }

        UniTask IOverlayModule.PlayOutAnimation()
        {
            return overlayModuleScene.PlayOutAnimation();
        }
    }
}