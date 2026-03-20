using System;
using Lighthouse.Scene.SceneTransitionPhase;

namespace Lighthouse.Scene
{
    public interface ISceneManager
    {
        public ISceneTransitionPhase CurrentTransitionPhase { get; }
        public bool IsTransition { get; }

        public void TransitionScene(
            TransitionDataBase nextTransitionData,
            TransitionType transitionType = TransitionType.Auto,
            MainSceneId backMainSceneId = null,
            Action<bool> onComplete = null);

        public void BackScene(TransitionType transitionType = TransitionType.Auto);
    }
}