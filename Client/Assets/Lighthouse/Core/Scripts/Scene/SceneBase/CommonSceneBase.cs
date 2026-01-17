using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Lighthouse.Core.Scene.SceneBase
{
    [RequireComponent(typeof(TransitionAnimatorManager))]
    public abstract class CommonSceneBase : SceneBase
    {
        [SerializeField] TransitionAnimatorManager transitionAnimatorManager;

        bool initialized;

        public abstract CommonSceneKey CommonSceneId { get; }

        public VisibleStateType VisibleStateType { get; protected set; }

        public virtual bool IsAlwaysInAnimation { get; protected set; } = false;
        public virtual bool IsAlwaysOutAnimation { get; protected set; } = false;

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

        protected override void OnBeginInAnimation(TransitionType transitionType, bool withStateChange)
        {
            base.OnBeginInAnimation(transitionType, withStateChange);

            VisibleStateType = VisibleStateType.Showing;
        }

        protected override async UniTask InAnimation(TransitionType transitionType, bool withStateChange)
        {
            await transitionAnimatorManager.InAnimation(transitionType);
        }

        protected override void OnCompleteInAnimation(TransitionType transitionType, bool withStateChange)
        {
            VisibleStateType = VisibleStateType.Visible;
        }

        protected override void OnBeginOutAnimation(TransitionType transitionType, bool withStateChange)
        {
            VisibleStateType = VisibleStateType.Hiding;
        }

        protected override async UniTask OutAnimation(TransitionType transitionType, bool withStateChange)
        {
            await transitionAnimatorManager.OutAnimation(transitionType);
        }

        protected override void OnCompleteOutAnimation(TransitionType transitionType, bool withStateChange)
        {
            base.OnCompleteOutAnimation(transitionType, withStateChange);

            VisibleStateType = VisibleStateType.Hidden;
        }

        protected virtual UniTask Setup()
        {
            return UniTask.CompletedTask;
        }

        public virtual void OnSceneTransitionFinished(MainSceneKey mainSceneKey)
        {
        }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            transitionAnimatorManager ??= GetComponent<TransitionAnimatorManager>();
        }
#endif
    }
}