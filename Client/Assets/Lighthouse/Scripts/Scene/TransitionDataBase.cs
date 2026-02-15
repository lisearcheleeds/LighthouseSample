using System.Threading;
using Cysharp.Threading.Tasks;

namespace Lighthouse.Scene
{
    public abstract class TransitionDataBase
    {
        public abstract MainSceneId MainSceneId { get; }

        public bool CanTransition { get; protected set; } = true;

        public virtual UniTask LoadSceneState(TransitionType transitionType, CancellationToken cancelToken)
        {
            return UniTask.CompletedTask;
        }
    }
}