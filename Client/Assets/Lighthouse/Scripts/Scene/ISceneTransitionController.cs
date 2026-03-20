using System.Threading;
using Cysharp.Threading.Tasks;

namespace Lighthouse.Scene
{
    public interface ISceneTransitionController
    {
        UniTask<bool> StartTransitionSequence(
            TransitionDataBase transitionData,
            SceneTransitionDiff sceneTransitionDiff,
            TransitionDirectionType transitionDirectionType,
            TransitionType transitionType,
            CancellationToken cancellationToken);
    }
}