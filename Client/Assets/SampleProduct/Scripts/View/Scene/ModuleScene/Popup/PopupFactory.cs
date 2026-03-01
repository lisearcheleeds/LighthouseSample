using System.Linq;
using Cysharp.Threading.Tasks;
using LighthouseExtends.Popup;
using SampleProduct.View.Common.Popup.PopupTest;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace SampleProduct.View.Scene.ModuleScene.Popup
{
    public sealed class PopupFactory : IPopupFactory
    {
        IObjectResolver objectResolver;

        [Inject]
        public void Construct(IObjectResolver objectResolver)
        {
            this.objectResolver = objectResolver;
        }

        async UniTask<IPopup> IPopupFactory.CreatePopup(IPopupData popupData)
        {
            return popupData switch
            {
                PopupTest1Data => await LoadPopupAsset("PopupTest1"),
            };
        }

        // I should do it properly.
        async UniTask<IPopup> LoadPopupAsset(string popupAddress)
        {
            var request = Resources.LoadAsync<GameObject>(popupAddress);
            await request.ToUniTask();
            var prefab = request.asset as GameObject;
            var gameObject = objectResolver.Instantiate(prefab);
            return gameObject.GetComponents<MonoBehaviour>().OfType<IPopup>().First();
        }
    }
}