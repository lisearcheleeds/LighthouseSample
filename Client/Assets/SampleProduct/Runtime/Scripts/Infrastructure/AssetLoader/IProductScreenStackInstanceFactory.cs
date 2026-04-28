using LighthouseExtends.ScreenStack;

namespace SampleProduct.Infrastructure.AssetLoader
{
    public interface IProductScreenStackInstanceFactory : IScreenStackInstanceFactory
    {
        void DisposeScope(IScreenStackData data);
        void DisposeAllScopes();
    }
}
