using System;
using Cysharp.Threading.Tasks;
using Lighthouse.Core.Scene;
using SampleProduct.View.Scene.SceneModule.Overlay;
using UnityEngine;
using UnityEngine.UI;

namespace SampleProduct.View.Scene.MainScene.Splash
{
    public class SplashView : MonoBehaviour
    {
        [SerializeField] Image splashImage;

        [SerializeField] Sprite splashImage1;
        [SerializeField] Sprite splashImage2;

        OverlaySceneModule overlaySceneModule;

        public void SetupFirstSplashImage(OverlaySceneModule overlaySceneModule)
        {
            this.overlaySceneModule = overlaySceneModule;

            splashImage.sprite = splashImage1;
        }

        public void PlaySplashAnimation(Action onComplete)
        {
            PlaySplashAnimationAsync(onComplete).Forget();
        }

        async UniTask PlaySplashAnimationAsync(Action onComplete)
        {
            await UniTask.Delay(1500);

            await overlaySceneModule.PlayOutAnimation(TransitionType.Default, false);

            splashImage.sprite = splashImage2;

            await UniTask.Delay(100);

            await overlaySceneModule.PlayInAnimation(TransitionType.Default, false);

            await UniTask.Delay(1500);
            onComplete();
        }
    }
}