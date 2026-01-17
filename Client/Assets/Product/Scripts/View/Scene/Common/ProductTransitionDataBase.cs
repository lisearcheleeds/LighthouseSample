using Lighthouse.Core.Scene;
using ProductNameSpace;

namespace Product.View.Scene.Common
{
    public abstract class ProductTransitionDataBase : TransitionDataBase
    {
        protected override CommonSceneKey[] MustRequireCommonSceneIds { get; } = { CommonSceneId.Overlay };
    }
}