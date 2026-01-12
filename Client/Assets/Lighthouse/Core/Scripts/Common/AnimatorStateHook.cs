using System.Linq;
using UnityEngine;

namespace Product.Util
{
    public class AnimatorStateHook : StateMachineBehaviour
    {
        INotifyStateEnterHolder[] notifyStateEnterHolders;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            notifyStateEnterHolders ??= animator.GetComponents<MonoBehaviour>().OfType<INotifyStateEnterHolder>().ToArray();
            foreach (var notifyStateEnterHolder in notifyStateEnterHolders)
            {
                notifyStateEnterHolder.NotifyStateEnter(animator, layerIndex, stateInfo.shortNameHash, stateInfo);
            }
        }
    }
}