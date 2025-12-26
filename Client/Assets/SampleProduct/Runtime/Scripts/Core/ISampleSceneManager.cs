using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Scene;

namespace SampleProduct.Core
{
    public interface ISampleSceneManager
    {
        bool IsTransition { get; }

        UniTask TransitionScene(
            TransitionDataBase nextTransitionData,
            TransitionType transitionType = TransitionType.Auto,
            MainSceneId backMainSceneId = default);

        UniTask BackScene(TransitionType transitionType = TransitionType.Auto);

        UniTask PreReboot();
    }
}
