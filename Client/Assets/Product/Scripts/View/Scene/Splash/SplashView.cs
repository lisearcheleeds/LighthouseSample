using System;
using Cysharp.Threading.Tasks;
using Lighthouse.Core.Scene;
using Product.View.Scene.Overlay;
using UnityEngine;
using UnityEngine.UI;

namespace Product.View.Scene.Splash
{
    public class SplashView : MonoBehaviour
    {
        [SerializeField] Image splashImage;

        [SerializeField] Sprite splashImage1;
        [SerializeField] Sprite splashImage2;

        OverlayScene overlayScene;

        public void SetupFirstSplashImage(OverlayScene overlayScene)
        {
            this.overlayScene = overlayScene;

            splashImage.sprite = splashImage1;
        }

        public void PlaySplashAnimation(Action onComplete)
        {
            PlaySplashAnimationAsync(onComplete).Forget();
        }

        async UniTask PlaySplashAnimationAsync(Action onComplete)
        {
            await UniTask.Delay(1500);

            await overlayScene.PlayOutAnimation(TransitionType.Default, false);

            splashImage.sprite = splashImage2;

            await UniTask.Delay(100);

            await overlayScene.PlayInAnimation(TransitionType.Default, false);

            await UniTask.Delay(1500);
            onComplete();
        }
    }
}