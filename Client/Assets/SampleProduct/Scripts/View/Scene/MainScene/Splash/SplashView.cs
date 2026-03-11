using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SampleProduct.View.Scene.ModuleScene.Overlay;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.Splash
{
    public class SplashView : MonoBehaviour, ISplashView
    {
        [SerializeField] Image splashImage;

        [SerializeField] Sprite splashImage1;
        [SerializeField] Sprite splashImage2;

        IOverlayModule overlayModule;

        [Inject]
        public void Construct(IOverlayModule overlayModule)
        {
            this.overlayModule = overlayModule;
            splashImage.sprite = splashImage1;
        }

        async UniTask ISplashView.PlaySplashAnimationAsync()
        {
            await UniTask.Delay(1500);

            await overlayModule.PlayOutAnimation(false);

            splashImage.sprite = splashImage2;

            await UniTask.Delay(100);

            await overlayModule.PlayInAnimation(false);

            await UniTask.Delay(1500);
        }
    }
}