using System.Linq;
using Cysharp.Threading.Tasks;
using LighthouseExtends.ScreenStack;
using SampleProduct.Infrastructure.AssetLoader;
using VContainer;

namespace SampleProduct.View.Scene.ModuleScene.ScreenStack
{
    public class ProductScreenStackManager : ScreenStackManager
    {
        IProductScreenStackInstanceFactory productFactory;

        [Inject]
        public void Construct(IProductScreenStackInstanceFactory productFactory)
        {
            this.productFactory = productFactory;
        }

        protected override async UniTask OpenScreenStackCore(bool isPlayInAnimation)
        {
            var data = ScreenStackDataList?.LastOrDefault();
            try
            {
                await base.OpenScreenStackCore(isPlayInAnimation);
            }
            catch
            {
                if (data != null)
                {
                    productFactory.DisposeScope(data);
                }
                throw;
            }
        }

        protected override async UniTask CloseScreenStackCore(IScreenStackData screenStackData)
        {
            await base.CloseScreenStackCore(screenStackData);
            productFactory.DisposeScope(screenStackData);
        }

        protected override async UniTask CloseScreenStackCore()
        {
            var data = ScreenStackDataList?.LastOrDefault();
            await base.CloseScreenStackCore();
            if (data != null)
            {
                productFactory.DisposeScope(data);
            }
        }

        protected override async UniTask ClearCurrentAllScreenStackCore()
        {
            await base.ClearCurrentAllScreenStackCore();
            productFactory.DisposeAllScopes();
        }

        protected override void ForceDisposeAll()
        {
            base.ForceDisposeAll();
            productFactory.DisposeAllScopes();
        }
    }
}
