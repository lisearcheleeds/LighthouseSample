using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using LighthouseExtends.ScreenStack;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace SampleProduct.Infrastructure.AssetLoader
{
    public sealed class AssetLoader : IScreenStackInstanceFactory
    {
        readonly IObjectResolver objectResolver;

        [Inject]
        public AssetLoader(IObjectResolver objectResolver)
        {
            this.objectResolver = objectResolver;
        }

        async UniTask<TScreenStack> IScreenStackInstanceFactory.CreateScreenStackInstance<TScreenStack>(string screenStackAddress, CancellationToken ct)
        {
            var request = Resources.LoadAsync<GameObject>(screenStackAddress);
            await request.ToUniTask(cancellationToken: ct);
            var prefab = request.asset as GameObject;
            var gameObject = objectResolver.Instantiate(prefab);
            return gameObject.GetComponents<MonoBehaviour>().OfType<TScreenStack>().First();
        }
    }
}