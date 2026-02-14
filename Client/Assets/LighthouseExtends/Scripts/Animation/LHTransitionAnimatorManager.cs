using System.Linq;
using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
using UnityEngine;

namespace LighthouseExtends.Animation
{
    public class LHTransitionAnimatorManager : MonoBehaviour
    {
        ITransitionAnimator[] sceneTransitionAnimatorList;

        public void ResetInAnimation(TransitionType transitionType)
        {
            foreach (var sceneTransitionAnimator in sceneTransitionAnimatorList)
            {
                sceneTransitionAnimator.ResetInAnimation();
            }
        }

        public async UniTask InAnimation(TransitionType transitionType)
        {
            await UniTask.WhenAll(sceneTransitionAnimatorList.Select(x => x.InAnimation()));
        }

        public async UniTask OutAnimation(TransitionType transitionType)
        {
            await UniTask.WhenAll(sceneTransitionAnimatorList.Select(x => x.OutAnimation()));
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            sceneTransitionAnimatorList = GetComponentsInChildren<MonoBehaviour>().OfType<ITransitionAnimator>().ToArray();

            if (sceneTransitionAnimatorList.Length != 0)
            {
                Debug.Log($"[LHTransitionAnimatorManager] Collect ITransitionAnimator {sceneTransitionAnimatorList.Length}");
            }
        }
#endif
    }
}