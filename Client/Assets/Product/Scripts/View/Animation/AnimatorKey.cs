using UnityEngine;

namespace Product.View.Animation
{
    public static class AnimatorKey
    {
        public static int Play = Animator.StringToHash("Play");

        public static int Reset = Animator.StringToHash("Reset");
        public static int In = Animator.StringToHash("In");
        public static int Out = Animator.StringToHash("Out");
    }
}