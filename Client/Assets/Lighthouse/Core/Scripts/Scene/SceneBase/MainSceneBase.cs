using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Lighthouse.Core.Scene.SceneBase
{
    public abstract class MainSceneBase : SceneBase
    {
        public abstract MainSceneKey MainSceneId { get; }
    }

    [RequireComponent(typeof(TransitionAnimatorManager))]
    public abstract class MainSceneBase<TTransitionData> : MainSceneBase where TTransitionData : TransitionDataBase, new()
    {
        [SerializeField] TransitionAnimatorManager transitionAnimatorManager;

        bool initialized;

        protected TTransitionData TransitionData { get; private set; }

        public override UniTask OnLoad()
        {
            gameObject.SetActive(false);
            return UniTask.CompletedTask;
        }

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

        protected override async UniTask InAnimation(TransitionType transitionType, bool withStateChange)
        {
            await transitionAnimatorManager.InAnimation(transitionType);
        }

        protected override async UniTask OutAnimation(TransitionType transitionType, bool withStateChange)
        {
            await transitionAnimatorManager.OutAnimation(transitionType);
        }

        protected virtual UniTask Setup()
        {
            return UniTask.CompletedTask;
        }

        protected virtual void OnBackKeyFallback()
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