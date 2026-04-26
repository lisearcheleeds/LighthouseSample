using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using LighthouseExtends.ScreenStack;
using SampleProduct.Infrastructure.AssetLoader;
using VContainer;

namespace SampleProduct.View.Scene.ModuleScene.ScreenStack
{
    public class ProductScreenStackManager : ScreenStackManager
    {
        ProductScreenStackInstanceFactory productFactory;

        [Inject]
        public void Construct(ProductScreenStackInstanceFactory productFactory)
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

        // SetPendingData must be called before each CreateAsync; cannot delegate to base.
        protected override async UniTask ResumeOpenScreenStacksCore(bool isPlayInAnimation)
        {
            try
            {
                for (var i = 0; i < ScreenStackDataList.Count; i++)
                {
                    var screenStackData = ScreenStackDataList[i];
                    var shouldPlayAnimation = isPlayInAnimation && i == ScreenStackDataList.Count - 1;

                    var prevScreenStackEntity = ScreenStackEntityList.LastOrDefault();
                    if (prevScreenStackEntity?.ScreenStackData == screenStackData)
                    {
                        throw new InvalidOperationException($"Duplicate open");
                    }

                    var screenStackEntity = await ScreenStackEntityFactory.CreateAsync(screenStackData, CancellationToken.None);

                    if (shouldPlayAnimation)
                    {
                        screenStackEntity.ScreenStack.ResetInAnimation();
                    }
                    else
                    {
                        screenStackEntity.ScreenStack.EndInAnimation();
                    }

                    ScreenStackEntityList.Add(screenStackEntity);
                    ScreenStackCanvasController.AddChild(screenStackEntity.ScreenStack, screenStackData.IsSystem);

                    await screenStackEntity.ScreenStack.OnInitialize();

                    if (prevScreenStackEntity != null)
                    {
                        if (!screenStackData.IsOverlayOpen)
                        {
                            prevScreenStackEntity.ScreenStack.EndOutAnimation();
                        }

                        await prevScreenStackEntity.ScreenStack.OnLeave();
                    }

                    await screenStackEntity.ScreenStack.OnEnter(false);

                    if (shouldPlayAnimation)
                    {
                        await screenStackEntity.ScreenStack.PlayInAnimation();
                    }

                    ScreenStackBackgroundInputBlocker.BlockScreenStackBackground(screenStackData.IsSystem);
                }
            }
            catch (Exception)
            {
                await ClearCurrentAllScreenStackCore();
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
