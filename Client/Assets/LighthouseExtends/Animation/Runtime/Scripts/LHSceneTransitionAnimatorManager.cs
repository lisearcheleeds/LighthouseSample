using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LighthouseExtends.Animation.Runtime
{
    public class LHSceneTransitionAnimatorManager : MonoBehaviour
    {
        [SerializeField] MonoBehaviour[] sceneTransitionAnimatorList;

        public void ResetInAnimation()
        {
            foreach (var sceneTransitionAnimator in sceneTransitionAnimatorList)
            {
                ((ILHSceneTransitionAnimator)sceneTransitionAnimator).ResetInAnimation();
            }
        }

        public async UniTask InAnimation()
        {
            await UniTask.WhenAll(sceneTransitionAnimatorList.Select(x => ((ILHSceneTransitionAnimator)x).InAnimation()));
        }

        public async UniTask OutAnimation()
        {
            await UniTask.WhenAll(sceneTransitionAnimatorList.Select(x => ((ILHSceneTransitionAnimator)x).OutAnimation()));
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            var beforeCount = sceneTransitionAnimatorList?.Length ?? 0;
            sceneTransitionAnimatorList = GetComponentsInChildren<MonoBehaviour>()
                .Where(x => x is ILHSceneTransitionAnimator)
                .ToArray();

            if (beforeCount != sceneTransitionAnimatorList.Length)
            {
                Debug.Log($"[LHSceneTransitionAnimatorManager] Collect ILHSceneTransitionAnimator {sceneTransitionAnimatorList.Length}");
            }
        }
#endif
    }
}