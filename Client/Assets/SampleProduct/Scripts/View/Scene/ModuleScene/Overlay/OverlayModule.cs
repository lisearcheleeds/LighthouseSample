using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
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

        UniTask IOverlayModule.PlayInAnimation(bool withStateChange)
        {
            return overlayModuleScene.PlayInAnimation(TransitionType.Default, withStateChange);
        }

        UniTask IOverlayModule.PlayOutAnimation(bool withStateChange)
        {
            return overlayModuleScene.PlayOutAnimation(TransitionType.Default, withStateChange);
        }
    }
}