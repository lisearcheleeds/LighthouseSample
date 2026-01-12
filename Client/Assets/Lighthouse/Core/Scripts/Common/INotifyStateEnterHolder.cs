using UnityEngine;

namespace Product.Util
{
    internal interface INotifyStateEnterHolder
    {
        void NotifyStateEnter(Animator animator, int layer, int stateShortNameHash, AnimatorStateInfo info);
    }
}