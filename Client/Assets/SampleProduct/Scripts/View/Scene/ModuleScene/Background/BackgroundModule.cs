using Cysharp.Threading.Tasks;
using VContainer;

namespace SampleProduct.View.Scene.ModuleScene.Background
{
    public class BackgroundModule : IBackgroundModuleImpl
    {
        readonly BackgroundModuleScene backgroundModuleScene;

        [Inject]
        public BackgroundModule(BackgroundModuleScene backgroundModuleScene)
        {
            this.backgroundModuleScene = backgroundModuleScene;
        }

        public UniTask SetBackground(string backgroundAsset)
        {
            return backgroundModuleScene.SetBackground(backgroundAsset);
        }
    }
}