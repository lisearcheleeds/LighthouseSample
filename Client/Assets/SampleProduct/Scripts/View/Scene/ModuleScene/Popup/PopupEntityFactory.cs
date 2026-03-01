using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using LighthouseExtends.Popup;
using SampleProduct.View.Common.Popup.PopupTest;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace SampleProduct.View.Scene.ModuleScene.Popup
{
    public sealed class PopupEntityFactory : IPopupEntityFactory
    {
        readonly IObjectResolver objectResolver;

        public PopupEntityFactory(IObjectResolver objectResolver)
        {
            this.objectResolver = objectResolver;
        }

        public UniTask<PopupEntity> CreateAsync(IPopupData data, CancellationToken ct)
        {
            return data switch
            {
                PopupSample1PopupData d => CreatePopupEntityAsync<PopupSample1Popup, PopupSample1PopupPresenter, PopupSample1PopupData>("PopupSample1Popup", d, ct),
                _ => throw new ArgumentOutOfRangeException(nameof(data), data.GetType().FullName, "Unknown popup data type")
            };
        }

        async UniTask<PopupEntity> CreatePopupEntityAsync<TPopup, TPopupPresenter, TPopupData>(
            string popupAddress,
            TPopupData data,
            CancellationToken ct)
            where TPopup : IPopup, IPopupSetup<TPopupPresenter, TPopupData>
            where TPopupPresenter : IPopupPresenter, new()
            where TPopupData : IPopupData
        {
            var popup = await LoadPopupAsset<TPopup>(popupAddress, ct);
            var popupPresenter = new TPopupPresenter();
            objectResolver.Inject(popupPresenter);

            popup.Setup(popupPresenter, data);

            return new PopupEntity(popup, popupPresenter, data);
        }

        // TODO: I should do it properly. Inject dependencies.
        async UniTask<TPopup> LoadPopupAsset<TPopup>(string popupAddress, CancellationToken ct)
        {
            var request = Resources.LoadAsync<GameObject>(popupAddress);
            await request.ToUniTask();
            var prefab = request.asset as GameObject;
            var gameObject = objectResolver.Instantiate(prefab);
            return gameObject.GetComponents<MonoBehaviour>().OfType<TPopup>().First();
        }
    }
}