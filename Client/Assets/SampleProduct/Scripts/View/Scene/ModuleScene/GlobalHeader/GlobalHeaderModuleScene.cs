using Lighthouse.Scene;
using SampleProduct.View.Base;
using UnityEngine;

namespace SampleProduct.View.Scene.ModuleScene.GlobalHeader
{
    public class GlobalHeaderModuleScene : ProductCanvasModuleSceneBase
    {
        [SerializeField] GlobalHeaderView globalHeaderView;

        public override ModuleSceneId ModuleSceneId => SampleProductModuleSceneId.GlobalHeader;

        public override bool IsAlwaysInAnimation => true;
        public override bool IsAlwaysOutAnimation => true;

        public void SetHeaderText(string text)
        {
            globalHeaderView.SetText(text);
        }
    }
}