using Lighthouse.Core.Scene;
using SampleProduct.View.Scene.SceneBase;
using UnityEngine;

namespace SampleProduct.View.Scene.ModuleScene.GlobalHeader
{
    public class GlobalHeaderModuleScene : ProductCanvasModuleSceneBase
    {
        [SerializeField] GlobalHeaderView globalHeaderView;

        public override ModuleSceneId ModuleSceneId => SampleProductModuleSceneId.GlobalHeader;

        public void SetText(string text)
        {
            globalHeaderView.SetText(text);
        }
    }
}