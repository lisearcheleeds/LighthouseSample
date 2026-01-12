using UnityEngine;

namespace Lighthouse.Core.Common
{
    internal interface INotifyStateEnterHolder
    {
        void NotifyStateEnter(Animator animator, int layer, int stateShortNameHash, AnimatorStateInfo info);
    }
}