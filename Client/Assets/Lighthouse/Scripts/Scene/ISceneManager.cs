using System;
using Cysharp.Threading.Tasks;

namespace Lighthouse.Scene
{
    public interface ISceneManager
    {
        bool IsTransition { get; }

        void TransitionScene(
            TransitionDataBase nextTransitionData,
            TransitionType transitionType = TransitionType.Auto,
            MainSceneId backMainSceneId = null,
            Action<bool> onComplete = null);

        void BackScene(TransitionType transitionType = TransitionType.Auto);
        UniTask PreReboot();
    }
}