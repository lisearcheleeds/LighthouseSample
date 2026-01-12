using System.Linq;
using Cysharp.Threading.Tasks;
using Product.Util;
using UnityEngine;

namespace Lighthouse.Core.Scene
{
    public class SceneTransitionAnimatorManager : MonoBehaviour
    {
        ISceneTransitionAnimator[] sceneTransitionAnimatorList;

        public async UniTask ResetAnimation(TransitionType transitionType)
        {
            await UniTask.WhenAll(sceneTransitionAnimatorList.Select(x => x.ResetAnimation()));
        }

        public async UniTask In(TransitionType transitionType)
        {
            await UniTask.WhenAll(sceneTransitionAnimatorList.Select(x => x.InAnimation()));
        }

        public async UniTask Out(TransitionType transitionType)
        {
            await UniTask.WhenAll(sceneTransitionAnimatorList.Select(x => x.OutAnimation()));
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            sceneTransitionAnimatorList = GetComponentsInChildren<MonoBehaviour>().OfType<ISceneTransitionAnimator>().ToArray();

            if (sceneTransitionAnimatorList.Length != 0)
            {
                Debug.Log($"[SceneTransitionAnimatorManager] Collect ISceneTransitionAnimator {sceneTransitionAnimatorList.Length}");
            }
        }
#endif
    }
}