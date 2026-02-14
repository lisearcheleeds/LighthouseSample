using System.Threading;
using Cysharp.Threading.Tasks;

namespace Lighthouse.Scene
{
    public abstract class TransitionDataBase
    {
        public abstract MainSceneId MainSceneId { get; }

        public virtual bool CanTransition()
        {
            return true;
        }

        public virtual UniTask LoadSceneState(TransitionType transitionType, CancellationToken cancelToken)
        {
            return UniTask.CompletedTask;
        }
    }
}