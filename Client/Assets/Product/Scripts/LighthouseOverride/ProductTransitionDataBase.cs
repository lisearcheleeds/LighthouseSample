using Lighthouse.Core.Scene;
using ProductNameSpace;

namespace Product.LighthouseOverride
{
    public abstract class ProductTransitionDataBase : TransitionDataBase
    {
        protected override CommonSceneKey[] MustRequireCommonSceneIds { get; } = { CommonSceneId.Overlay };
    }
}