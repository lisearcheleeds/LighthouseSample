using System.Threading;
using Cysharp.Threading.Tasks;

namespace Lighthouse.Core.Scene.SceneBase
{
    public abstract class MainSceneBase : SceneBase
    {
        public abstract MainSceneKey MainSceneId { get; }
    }

    public abstract class MainSceneBase<TTransitionData> : MainSceneBase where TTransitionData : TransitionDataBase, new()
    {
        protected TTransitionData TransitionData { get; private set; }

        protected override UniTask OnEnter(TransitionDataBase transitionData, TransitionType transitionType, CancellationToken cancelToken)
        {
            TransitionData = (TTransitionData)transitionData;
            return UniTask.CompletedTask;
        }

        protected virtual void OnBackKeyFallback()
        {
        }
    }
}