using System.Threading;
using Cysharp.Threading.Tasks;

namespace Lighthouse.Scene
{
    public abstract class TransitionDataBase
    {
        public abstract MainSceneId MainSceneId { get; }

        public bool CanTransition { get; protected set; } = true;

        public bool CanBackTransition { get; protected set; } = true;

        public virtual UniTask LoadSceneState(TransitionDirectionType transitionDirectionType, CancellationToken cancelToken)
        {
            return UniTask.CompletedTask;
        }
    }
}