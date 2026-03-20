using System.Linq;
using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
using UnityEngine;

namespace LighthouseExtends.Animation
{
    public class LHTransitionAnimatorManager : MonoBehaviour
    {
        ILHTransitionAnimator[] sceneTransitionAnimatorList;

        public void ResetInAnimation()
        {
            foreach (var sceneTransitionAnimator in sceneTransitionAnimatorList)
            {
                sceneTransitionAnimator.ResetInAnimation();
            }
        }

        public async UniTask InAnimation()
        {
            await UniTask.WhenAll(sceneTransitionAnimatorList.Select(x => x.InAnimation()));
        }

        public async UniTask OutAnimation()
        {
            await UniTask.WhenAll(sceneTransitionAnimatorList.Select(x => x.OutAnimation()));
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            sceneTransitionAnimatorList = GetComponentsInChildren<MonoBehaviour>().OfType<ILHTransitionAnimator>().ToArray();

            if (sceneTransitionAnimatorList.Length != 0)
            {
                Debug.Log($"[LHTransitionAnimatorManager] Collect ILHTransitionAnimator {sceneTransitionAnimatorList.Length}");
            }
        }
#endif
    }
}