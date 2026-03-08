using System;
using Cysharp.Threading.Tasks;

namespace SampleProduct.View.Scene.MainScene.Splash
{
    public interface ISplashView
    {
        UniTask PlaySplashAnimationAsync();
    }
}