using UnityEngine;

namespace LighthouseExtends.Animation.Runtime
{
    public class LHSceneTransitionAnimator : LHTransitionAnimator, ILHSceneTransitionAnimator
    {
        [SerializeField] bool invokeResetInAnimation = true;
        [SerializeField] bool invokeInAnimation = true;
        [SerializeField] bool invokeOutAnimation = true;

        public bool InvokeResetInAnimation => invokeResetInAnimation;
        public bool InvokeInAnimation => invokeInAnimation;
        public bool InvokeOutAnimation => invokeOutAnimation;
    }
}