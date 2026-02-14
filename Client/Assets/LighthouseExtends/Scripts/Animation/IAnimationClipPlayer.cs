using System;
using Cysharp.Threading.Tasks;

namespace LighthouseExtends.Animation
{
    public interface IAnimationClipPlayer
    {
        public void PlayAnimation(bool isRestart, bool isEndEvaluate, Action onComplete);
        public UniTask PlayAnimationAsync(bool isRestart, bool isEndEvaluate);
        public void ResetAnimation();
        public void Stop();
        public void Skip();
    }
}