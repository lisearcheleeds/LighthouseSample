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

        void IBackgroundModule.SetBackgroundLayout(BackgroundLayout backgroundLayout)
        {
            backgroundModuleScene.SetBackgroundLayout(backgroundLayout);
        }
    }
}