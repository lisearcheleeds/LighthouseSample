using System;
using UnityEngine;

namespace Product.View.Animation
{
    public class AnimatorTrigger : MonoBehaviour, INotifyStateEnterHolder
    {
        [SerializeField] Animator animator;

        Action onComplete;
        Action onBreak;
        int endStateHash;
        int layer;

#if UNITY_EDITOR
        void OnValidate()
        {
            animator = GetComponent<Animator>();
        }
#endif

        public void SetTrigger(int triggerHash, int endStateHash, int layer = -1, Action onComplete = null, Action onBreak = null)
        {
            this.onBreak?.Invoke();

            this.onComplete = onComplete;
            this.onBreak = onBreak;
            this.endStateHash = endStateHash;
            this.layer = layer;

            animator.SetTrigger(triggerHash);
        }

        void INotifyStateEnterHolder.NotifyStateEnter(Animator animator, int layer, int stateShortNameHash, AnimatorStateInfo info)
        {
            if ((this.layer != -1 && this.layer != layer) || stateShortNameHash != endStateHash)
            {
                return;
            }

            var onCompleteCallback = onComplete;

            this.onComplete = null;
            this.onBreak = null;
            this.endStateHash = 0;
            this.layer = -1;

            onCompleteCallback?.Invoke();
        }
    }
}