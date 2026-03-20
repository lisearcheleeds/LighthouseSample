using Cysharp.Threading.Tasks;

namespace Lighthouse.Scene
{
    public interface ISceneManager
    {
        bool IsTransition { get; }

        UniTask TransitionScene(
            TransitionDataBase nextTransitionData,
            TransitionType transitionType = TransitionType.Auto,
            MainSceneId backMainSceneId = null);

        void BackScene(TransitionType transitionType = TransitionType.Auto);
        UniTask PreReboot();
    }
}