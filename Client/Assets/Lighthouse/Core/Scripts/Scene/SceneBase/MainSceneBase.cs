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
        bool initialized;

        protected TTransitionData TransitionData { get; private set; }

        public override async UniTask Enter(TransitionDataBase transitionData, TransitionType transitionType, CancellationToken cancelToken)
        {
            TransitionData = (TTransitionData)transitionData;

            if (!initialized)
            {
                initialized = true;
                await Setup();
            }

            gameObject.SetActive(true);
        }

        public override UniTask Leave(TransitionDataBase transitionData, TransitionType transitionType, CancellationToken cancelToken)
        {
            gameObject.SetActive(false);
            return UniTask.CompletedTask;
        }

        protected virtual UniTask Setup()
        {
            return UniTask.CompletedTask;
        }

        protected virtual void OnBackKeyFallback()
        {
        }
    }
}