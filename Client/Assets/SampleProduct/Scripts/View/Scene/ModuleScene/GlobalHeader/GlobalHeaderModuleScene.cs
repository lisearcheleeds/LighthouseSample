using Lighthouse.Core.Scene;
using ProductNameSpace;
using SampleProduct.View.Scene.SceneBase;
using UnityEngine;

namespace SampleProduct.View.Scene.ModuleScene.GlobalHeader
{
    public class GlobalHeaderModuleScene : ProductCanvasModuleSceneBase
    {
        [SerializeField] GlobalHeaderView globalHeaderView;

        public override ModuleSceneId ModuleSceneId => SampleProductSceneModuleId.GlobalHeader;

        public void SetText(string text)
        {
            globalHeaderView.SetText(text);
        }
    }
}