using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Lighthouse.Scene.SceneBase
{
    public abstract class MainSceneBase : SceneBase
    {
        public abstract MainSceneId MainSceneId { get; }
    }

    public abstract class MainSceneBase<TTransitionData> : MainSceneBase where TTransitionData : TransitionDataBase
    {
        protected TTransitionData TransitionData { get; private set; }

        protected override UniTask OnEnter(TransitionDataBase transitionData, TransitionType transitionType, SceneTransitionDiff sceneTransitionDiff, CancellationToken cancelToken)
        {
            // If necessary, you can override OnEnter to control the gameObject.
            gameObject.SetActive(true);

            TransitionData = (TTransitionData)transitionData;
            return base.OnEnter(transitionData, transitionType, sceneTransitionDiff, cancelToken);
        }

        protected override UniTask OnLeave(TransitionDataBase transitionData, TransitionType transitionType, SceneTransitionDiff sceneTransitionDiff, CancellationToken cancelToken)
        {
            gameObject.SetActive(false);

            return base.OnLeave(transitionData, transitionType, sceneTransitionDiff, cancelToken);
        }

        protected virtual void OnBackKeyFallback()
        {
        }
    }
}