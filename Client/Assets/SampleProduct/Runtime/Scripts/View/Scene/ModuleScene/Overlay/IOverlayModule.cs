using Cysharp.Threading.Tasks;

namespace SampleProduct.View.Scene.ModuleScene.Overlay
{
    public interface IOverlayModule
    {
        UniTask PlayInAnimation();
        UniTask PlayOutAnimation();
    }
}