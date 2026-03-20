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

        protected override UniTask OnEnter(SceneTransitionContext context, CancellationToken cancelToken)
        {
            // If necessary, you can override OnEnter to control the gameObject.
            gameObject.SetActive(true);

            TransitionData = (TTransitionData)context.TransitionData;
            return base.OnEnter(context, cancelToken);
        }

        protected override UniTask OnLeave(SceneTransitionContext context, CancellationToken cancelToken)
        {
            gameObject.SetActive(false);

            return base.OnLeave(context, cancelToken);
        }

        protected virtual void OnBackKeyFallback()
        {
        }
    }
}