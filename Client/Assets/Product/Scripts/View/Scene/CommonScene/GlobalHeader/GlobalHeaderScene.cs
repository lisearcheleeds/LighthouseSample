using Lighthouse.Core.Scene;
using Product.View.Scene.SceneBase;
using UnityEngine;

namespace Product.View.Scene.CommonScene.GlobalHeader
{
    public class GlobalHeaderScene : ProductCommonCanvasSceneBase
    {
        [SerializeField] GlobalHeaderView globalHeaderView;

        public override CommonSceneKey CommonSceneId => ProductNameSpace.CommonSceneId.GlobalHeader;

        public void SetText(string text)
        {
            globalHeaderView.SetText(text);
        }
    }
}