using Lighthouse.Core.Scene;
using ProductNameSpace;
using SampleProduct.View.Scene.SceneBase;
using UnityEngine;

namespace SampleProduct.View.Scene.SceneModule.GlobalHeader
{
    public class GlobalHeaderSceneModule : ProductCanvasSceneModuleBase
    {
        [SerializeField] GlobalHeaderView globalHeaderView;

        public override SceneModuleId SceneModuleId => SampleProductSceneModuleId.GlobalHeader;

        public void SetText(string text)
        {
            globalHeaderView.SetText(text);
        }
    }
}