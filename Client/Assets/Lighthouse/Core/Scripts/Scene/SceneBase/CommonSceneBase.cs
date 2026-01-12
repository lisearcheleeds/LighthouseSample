using System.Threading;
using Cysharp.Threading.Tasks;

namespace Lighthouse.Core.Scene.SceneBase
{
    public abstract class CommonSceneBase : SceneBase
    {
        bool initialized;

        public abstract CommonSceneKey CommonSceneId { get; }
        public VisibleStateType VisibleStateType { get; protected set; }

        public override async UniTask Enter(TransitionDataBase transitionData, TransitionType transitionType, CancellationToken cancelToken)
        {
            if (!initialized)
            {
                initialized = true;
                await Setup();
            }
        }

        public override UniTask Leave(TransitionDataBase transitionData, TransitionType transitionType, CancellationToken cancelToken)
        {
            return UniTask.CompletedTask;
        }

        protected override UniTask OnBeginInAnimation(TransitionType transitionType)
        {
            VisibleStateType = VisibleStateType.Showing;
            return base.OnBeginInAnimation(transitionType);
        }

        protected override UniTask OnCompleteInAnimation(TransitionType transitionType)
        {
            VisibleStateType = VisibleStateType.Visible;
            return base.OnCompleteInAnimation(transitionType);
        }

        protected override UniTask OnBeginOutAnimation(TransitionType transitionType)
        {
            VisibleStateType = VisibleStateType.Hiding;
            return base.OnBeginOutAnimation(transitionType);
        }

        protected override UniTask OnCompleteOutAnimation(TransitionType transitionType)
        {
            VisibleStateType = VisibleStateType.Hidden;
            return base.OnCompleteOutAnimation(transitionType);
        }

        protected virtual UniTask Setup()
        {
            return UniTask.CompletedTask;
        }

        public virtual void OnSceneTransitionFinished(MainSceneKey mainSceneKey)
        {
        }
    }
}