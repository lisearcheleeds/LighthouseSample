using System;
using Cysharp.Threading.Tasks;
using Product.Constant;
using Product.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Product.View.Splash
{
    public class SplashView : MonoBehaviour
    {
        [SerializeField] Image splashImage;

        [SerializeField] Sprite splashImage1;
        [SerializeField] Sprite splashImage2;

        [SerializeField] AnimatorTrigger animatorTrigger;

        public void SetupFirstSplashImage()
        {
            splashImage.sprite = splashImage1;
        }

        public void PlaySplashAnimation(Action onComplete)
        {
            PlaySplashAnimationAsync(onComplete).Forget();
        }

        async UniTask PlaySplashAnimationAsync(Action onComplete)
        {
            await UniTask.Delay(2000);

            var tcs = new UniTaskCompletionSource();
            animatorTrigger.SetTrigger(AnimatorKey.Out, AnimatorKey.EndState, onComplete: () => tcs.TrySetResult());
            await tcs.Task;

            splashImage.sprite = splashImage2;

            await UniTask.Delay(1000);

            var tcs2 = new UniTaskCompletionSource();
            animatorTrigger.SetTrigger(AnimatorKey.In, AnimatorKey.EndState, onComplete: () => tcs2.TrySetResult());
            await tcs2.Task;

            await UniTask.Delay(2000);
            onComplete();
        }
    }
}