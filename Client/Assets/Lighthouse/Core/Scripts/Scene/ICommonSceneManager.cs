using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Core.Scene.SceneBase;

namespace Lighthouse.Core.Scene
{
    public interface ICommonSceneManager
    {
        public ISceneCamera[] GetSceneCameraList(CommonSceneKey[] requestCommonSceneIds);

        public UniTask PlayResetAnimation(CommonSceneKey[] requestCommonSceneIds, TransitionType transitionType);
        public UniTask PlayInAnimation(CommonSceneKey[] requestCommonSceneIds, TransitionType transitionType);
        public UniTask PlayOutAnimation(CommonSceneKey[] requestCommonSceneIds, TransitionType transitionType);

        public UniTask LoadCommonScenes(CommonSceneKey[] requestCommonSceneIds);
        public UniTask UnloadUnusedCommonScenes(CommonSceneKey[] requireCommonSceneIds);

        public UniTask Enter(TransitionDataBase transitionData, TransitionType transitionType, CancellationToken cancellationToken);
        public UniTask Leave(TransitionDataBase transitionData, TransitionType transitionType, CancellationToken cancellationToken);

        public void InitializeCanvas(ISceneCamera sceneCamera, CommonSceneKey[] requestCommonSceneIds);

        public void OnSceneTransitionFinished(CommonSceneKey[] requestCommonSceneIds);

        public T GetCommonScene<T>() where T: CommonSceneBase;
    }
}