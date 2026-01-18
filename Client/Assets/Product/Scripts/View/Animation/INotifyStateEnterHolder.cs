using UnityEngine;

namespace Product.View.Animation
{
    internal interface INotifyStateEnterHolder
    {
        void NotifyStateEnter(Animator animator, int layer, int stateShortNameHash, AnimatorStateInfo info);
    }
}