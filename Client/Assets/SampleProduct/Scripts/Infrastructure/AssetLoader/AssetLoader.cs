using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using LighthouseExtends.Popup;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace SampleProduct.Infrastructure.AssetLoader
{
    public sealed class AssetLoader : IPopupInstanceFactory
    {
        readonly IObjectResolver objectResolver;

        [Inject]
        public AssetLoader(IObjectResolver objectResolver)
        {
            this.objectResolver = objectResolver;
        }

        async UniTask<TPopup> IPopupInstanceFactory.CreatePopupInstance<TPopup>(string popupAddress, CancellationToken ct)
        {
            var request = Resources.LoadAsync<GameObject>(popupAddress);
            await request.ToUniTask(cancellationToken: ct);
            var prefab = request.asset as GameObject;
            var gameObject = objectResolver.Instantiate(prefab);
            return gameObject.GetComponents<MonoBehaviour>().OfType<TPopup>().First();
        }
    }
}