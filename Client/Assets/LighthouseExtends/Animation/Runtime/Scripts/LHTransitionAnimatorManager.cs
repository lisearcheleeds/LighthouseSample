using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LighthouseExtends.Animation.Runtime
{
    public class LHTransitionAnimatorManager : MonoBehaviour
    {
        [SerializeField] MonoBehaviour[] sceneTransitionAnimatorList;

        public void ResetInAnimation()
        {
            foreach (var sceneTransitionAnimator in sceneTransitionAnimatorList)
            {
                ((ILHTransitionAnimator)sceneTransitionAnimator).ResetInAnimation();
            }
        }

        public async UniTask InAnimation()
        {
            await UniTask.WhenAll(sceneTransitionAnimatorList.Select(x => ((ILHTransitionAnimator)x).InAnimation()));
        }

        public async UniTask OutAnimation()
        {
            await UniTask.WhenAll(sceneTransitionAnimatorList.Select(x => ((ILHTransitionAnimator)x).OutAnimation()));
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            var beforeCount = sceneTransitionAnimatorList?.Length ?? 0;
            sceneTransitionAnimatorList = GetComponentsInChildren<MonoBehaviour>()
                .Where(x => x is ILHTransitionAnimator)
                .ToArray();

            if (beforeCount != sceneTransitionAnimatorList.Length)
            {
                Debug.Log($"[LHTransitionAnimatorManager] Collect ILHTransitionAnimator {sceneTransitionAnimatorList.Length}");
            }
        }
#endif
    }
}