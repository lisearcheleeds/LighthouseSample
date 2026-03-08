using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
using SampleProduct.View.Base;
using UnityEngine;

namespace SampleProduct.View.Scene.ModuleScene.Background
{
    public class BackgroundModuleScene : ProductCanvasModuleSceneBase
    {
        [SerializeField] BackgroundView backgroundView;

        public override ModuleSceneId ModuleSceneId => SampleProductModuleSceneId.Background;

        public async UniTask SetBackground(string backgroundAsset)
        {
            await backgroundView.SetBackground(backgroundAsset);
        }
    }
}