using VContainer;

namespace SampleProduct.View.Scene.ModuleScene.GlobalHeader
{
    public sealed class GlobalHeaderModule : IGlobalHeaderModuleImpl
    {
        readonly GlobalHeaderModuleScene globalHeaderModuleScene;

        [Inject]
        public GlobalHeaderModule(GlobalHeaderModuleScene globalHeaderModuleScene)
        {
            this.globalHeaderModuleScene = globalHeaderModuleScene;
        }

        public void SetHeaderText(string headerText)
        {
            globalHeaderModuleScene.SetHeaderText(headerText);
        }
    }
}