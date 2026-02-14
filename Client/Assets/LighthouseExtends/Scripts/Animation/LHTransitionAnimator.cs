using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LighthouseExtends.Animation
{
    public class LHTransitionAnimator : MonoBehaviour, ITransitionAnimator
    {
        [SerializeField] Animator animator;
        [SerializeField] AnimationClip[] inAnimationClips;
        [SerializeField] int inDelayMilliSec;
        [SerializeField] AnimationClip[] outAnimationClips;
        [SerializeField] int outDelayMilliSec;

        LHAnimationClipPlayer inAnimationClipPlayer;
        LHAnimationClipPlayer outAnimationClipPlayer;

        void InitializeIfNeeded()
        {
            if (inAnimationClipPlayer == null && inAnimationClips != null)
            {
                inAnimationClipPlayer = new LHAnimationClipPlayer(animator, inAnimationClips);
            }

            if (outAnimationClipPlayer == null && outAnimationClips != null)
            {
                outAnimationClipPlayer = new LHAnimationClipPlayer(animator, outAnimationClips);
            }
        }

        void ITransitionAnimator.ResetInAnimation()
        {
            InitializeIfNeeded();

            if (inAnimationClipPlayer == null)
            {
                return;
            }

            if (outAnimationClipPlayer != null)
            {
                outAnimationClipPlayer.Stop();
            }

            inAnimationClipPlayer.ResetAnimation();
        }

        async UniTask ITransitionAnimator.InAnimation()
        {
            InitializeIfNeeded();

            if (inAnimationClipPlayer == null)
            {
                return;
            }

            if (outAnimationClipPlayer != null)
            {
                outAnimationClipPlayer.Stop();
            }

            if (inDelayMilliSec != 0)
            {
                await UniTask.Delay(inDelayMilliSec);
            }

            await inAnimationClipPlayer.PlayAnimationAsync(true, true);
        }

        void ITransitionAnimator.EndInAnimation()
        {
            InitializeIfNeeded();

            if (inAnimationClipPlayer == null)
            {
                return;
            }

            if (outAnimationClipPlayer != null)
            {
                outAnimationClipPlayer.Stop();
            }

            inAnimationClipPlayer.Skip();
        }

        void ITransitionAnimator.ResetOutAnimation()
        {
            InitializeIfNeeded();

            if (outAnimationClipPlayer == null)
            {
                return;
            }

            if (inAnimationClipPlayer != null)
            {
                inAnimationClipPlayer.Stop();
            }

            outAnimationClipPlayer.ResetAnimation();
        }

        async UniTask ITransitionAnimator.OutAnimation()
        {
            InitializeIfNeeded();

            if (outAnimationClipPlayer == null)
            {
                return;
            }

            if (inAnimationClipPlayer != null)
            {
                inAnimationClipPlayer.Stop();
            }

            if (outDelayMilliSec != 0)
            {
                await UniTask.Delay(outDelayMilliSec);
            }

            await outAnimationClipPlayer.PlayAnimationAsync(true, true);
        }

        void ITransitionAnimator.EndOutAnimation()
        {
            InitializeIfNeeded();

            if (outAnimationClipPlayer == null)
            {
                return;
            }

            if (inAnimationClipPlayer != null)
            {
                inAnimationClipPlayer.Stop();
            }

            outAnimationClipPlayer.Skip();
        }

        void OnDestroy()
        {
            inAnimationClipPlayer?.Dispose();
            outAnimationClipPlayer?.Dispose();
        }
    }
}