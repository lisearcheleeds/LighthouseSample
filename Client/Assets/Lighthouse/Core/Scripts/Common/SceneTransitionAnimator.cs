using System;
using Cysharp.Threading.Tasks;
using Lighthouse.Core.Constant;
using UnityEngine;

namespace Lighthouse.Core.Common
{
    public class SceneTransitionAnimator : MonoBehaviour, ISceneTransitionAnimator, INotifyStateEnterHolder
    {
        [SerializeField] Animator resetAnimator;
        [SerializeField] Animator inAnimator;
        [SerializeField] Animator outAnimator;

        Animator currentAnimator;
        int currentLayer;
        int endStateHash;

        UniTaskCompletionSource completionSource;

        void INotifyStateEnterHolder.NotifyStateEnter(Animator animator, int layer, int stateShortNameHash, AnimatorStateInfo info)
        {
            if (completionSource == null)
            {
                return;
            }

            if (animator != currentAnimator || layer != currentLayer || stateShortNameHash != endStateHash)
            {
                return;
            }

            completionSource.TrySetResult();
        }

        async UniTask ISceneTransitionAnimator.ResetAnimation()
        {
            if (resetAnimator == null)
            {
                return;
            }

            await ExecuteAnimation(resetAnimator, AnimatorKey.Reset);
        }

        async UniTask ISceneTransitionAnimator.InAnimation()
        {
            if (inAnimator == null)
            {
                return;
            }

            await ExecuteAnimation(inAnimator, AnimatorKey.In);
        }

        async UniTask ISceneTransitionAnimator.OutAnimation()
        {
            if (outAnimator == null)
            {
                return;
            }

            await ExecuteAnimation(outAnimator, AnimatorKey.Out);
        }

        async UniTask ExecuteAnimation(Animator targetAnimator, int animatorKey)
        {
            completionSource?.TrySetCanceled();

            var tcs = new UniTaskCompletionSource();
            completionSource = tcs;

            currentAnimator = targetAnimator;
            currentLayer = 0;
            endStateHash = AnimatorKey.EndState;

            targetAnimator.SetTrigger(animatorKey);

            try
            {
                await tcs.Task;
            }
            catch (OperationCanceledException)
            {
                // Nothing
            }
            finally
            {
                if (completionSource == tcs)
                {
                    completionSource = null;
                    currentAnimator = null;
                    currentLayer = 0;
                    endStateHash = 0;
                }
            }
        }
    }
}