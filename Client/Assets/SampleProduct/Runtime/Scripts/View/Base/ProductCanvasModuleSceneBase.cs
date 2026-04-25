using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
using Lighthouse.Scene.SceneBase;
using LighthouseExtends.Addressable;
using LighthouseExtends.Animation;
using UnityEngine;
using VContainer;

namespace SampleProduct.View.Base
{
    [RequireComponent(typeof(LHSceneTransitionAnimatorManager))]
    public abstract class ProductCanvasModuleSceneBase : CanvasModuleSceneBase
    {
        [SerializeField] LHSceneTransitionAnimatorManager sceneTransitionAnimatorManager;

        ILHAssetManager assetManager;

        protected ILHAssetScope AssetScope { get; private set; }

        [Inject]
        public void Construct(ILHAssetManager assetManager)
        {
            this.assetManager = assetManager;
        }

        public override UniTask OnLoad()
        {
            AssetScope = assetManager.CreateScope();
            return base.OnLoad();
        }

        public override async UniTask OnUnload()
        {
            await base.OnUnload();
            AssetScope?.Dispose();
            AssetScope = null;
        }

        public override void ResetInAnimation(ISceneTransitionContext context)
        {
            sceneTransitionAnimatorManager.ResetInAnimation();
        }

        protected override async UniTask InAnimation(ISceneTransitionContext context)
        {
            await sceneTransitionAnimatorManager.InAnimation();
        }

        protected override async UniTask OutAnimation(ISceneTransitionContext context)
        {
            await sceneTransitionAnimatorManager.OutAnimation();
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            sceneTransitionAnimatorManager ??= GetComponent<LHSceneTransitionAnimatorManager>();
        }
#endif
    }
}
