using Cysharp.Threading.Tasks;

namespace SampleProduct.View.Scene.ModuleScene.Background
{
    public interface IBackgroundModule
    {
        UniTask SetBackground(string backgroundAsset);
    }
}