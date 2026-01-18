using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Product.View.Animation
{
    public class TransitionAnimator : MonoBehaviour, ITransitionAnimator
    {
        [SerializeField] Animator animator;
        [SerializeField] AnimationClip[] inAnimationClips;
        [SerializeField] AnimationClip[] outAnimationClips;

        AnimationClipPlayer inAnimationClipPlayer;
        AnimationClipPlayer outAnimationClipPlayer;

        void InitializeIfNeeded()
        {
            if (inAnimationClipPlayer == null && inAnimationClips != null)
            {
                inAnimationClipPlayer = new AnimationClipPlayer(animator, inAnimationClips);
            }

            if (outAnimationClipPlayer == null && outAnimationClips != null)
            {
                outAnimationClipPlayer = new AnimationClipPlayer(animator, outAnimationClips);
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
    }
}